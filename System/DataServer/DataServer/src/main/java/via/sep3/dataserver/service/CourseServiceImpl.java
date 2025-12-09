package via.sep3.dataserver.service;

import java.util.ArrayList;
import java.util.List;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.grpc.server.service.GrpcService;
import org.springframework.stereotype.Service;
import io.grpc.stub.StreamObserver;
import via.sep3.dataserver.data.Course;
import via.sep3.dataserver.data.SystemUser;
import via.sep3.dataserver.data.Language;
import via.sep3.dataserver.data.CourseCategory;
import via.sep3.dataserver.data.LearningStep;
import via.sep3.dataserver.data.LearningStepType;
import via.sep3.dataserver.data.LearningStepId;
import via.sep3.dataserver.data.UserCourseProgress;
import via.sep3.dataserver.data.*;
import via.sep3.dataserver.grpc.*;

@GrpcService
@Service
public class CourseServiceImpl extends CourseServiceGrpc.CourseServiceImplBase {

    @Autowired
    private CourseRepository courseRepository;
    @Autowired
    private LearningStepRepository learningStepRepository;
    @Autowired
    private LearningStepTypeRepository learningStepTypeRepository;
    @Autowired
    private LanguageRepository languageRepository;
    @Autowired
    private CourseCategoryRepository courseCategoryRepository;
    @Autowired
    private UserCourseProgressRepository progressRepository;

    @Autowired
    private SystemUserRepository userRepository;
    @Autowired
    private CourseCategoryRepository categoryRepository;

    @Override
    public void getCourses(GetCoursesRequest request, StreamObserver<GetCoursesResponse> responseObserver) {
        try {
            List<Course> courses;

            if (request.getUserId() > 0) {
                List<UserCourseProgress> progressList = progressRepository
                        .findBySystemUser_Id(request.getUserId());
                courses = new ArrayList<>();
                for (UserCourseProgress progress : progressList) {
                    courses.add(progress.getCourse());
                }
            } else {
                courses = courseRepository.findAll();
            }

            GetCoursesResponse.Builder responseBuilder = GetCoursesResponse.newBuilder();

            for (Course course : courses) {
                responseBuilder.addCourses(convertToGrpcCourse(course));
            }

            responseObserver.onNext(responseBuilder.build());
            responseObserver.onCompleted();

        } catch (Exception e) {
            responseObserver.onError(io.grpc.Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
        }
    }

    @Override
    public void addCourse(AddCourseRequest request, StreamObserver<AddCourseResponse> responseObserver) {
        try {
            Language language = languageRepository.findById(request.getLanguage()).orElse(null);
            if (language == null) {
                responseObserver
                        .onError(io.grpc.Status.NOT_FOUND.withDescription("Language not found").asRuntimeException());
                return;
            }

            CourseCategory category = courseCategoryRepository.findByName(request.getCategory());
            if (category == null) {
                responseObserver
                        .onError(io.grpc.Status.NOT_FOUND.withDescription("Category not found").asRuntimeException());
                return;
            }

            SystemUser author = userRepository.findById(request.getAuthorId()).orElse(null);
            if (author == null) {
                responseObserver.onError(
                        io.grpc.Status.NOT_FOUND.withDescription("Author (User) not found").asRuntimeException());
                return;
            }

            Course course = new Course();
            course.setTitle(request.getTitle());
            course.setDescription(request.getDescription());
            course.setLanguage(language);
            course.setCategory(category);
            course.setAuthor(author); // set the author
            course.setApprovedBy(null);

            course = courseRepository.save(course);

            AddCourseResponse response = AddCourseResponse.newBuilder()
                    .setCourse(convertToGrpcCourse(course))
                    .build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(e);
        }
    }

    @Override
    public void updateCourse(UpdateCourseRequest request, StreamObserver<UpdateCourseResponse> responseObserver) {
        try {
            var grpcCourse = request.getCourse();
            Course course = courseRepository.findById(grpcCourse.getId()).orElse(null);

            if (course == null) {
                responseObserver
                        .onError(io.grpc.Status.NOT_FOUND.withDescription("Course not found").asRuntimeException());
                return;
            }

            if (!grpcCourse.getTitle().isEmpty())
                course.setTitle(grpcCourse.getTitle());
            if (!grpcCourse.getDescription().isEmpty())
                course.setDescription(grpcCourse.getDescription());

            if (!grpcCourse.getLanguage().isEmpty()) {
                Language lang = languageRepository.findByName(grpcCourse.getLanguage());
                if (lang == null)
                    lang = languageRepository.findById(grpcCourse.getLanguage()).orElse(null);
                if (lang != null)
                    course.setLanguage(lang);
            }

            if (!grpcCourse.getCategory().isEmpty()) {
                CourseCategory cat = courseCategoryRepository.findByName(grpcCourse.getCategory());
                if (cat != null)
                    course.setCategory(cat);
            }

            if (grpcCourse.getApprovedBy() > 0) {
                SystemUser approver = userRepository.findById(grpcCourse.getApprovedBy()).orElse(null);
                if (approver != null) {
                    course.setApprovedBy(approver);
                }
            }

            courseRepository.save(course);

            UpdateCourseResponse response = UpdateCourseResponse.newBuilder()
                    .setCourse(convertToGrpcCourse(course))
                    .build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(io.grpc.Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
        }
    }

    @Override
    public void getLearningStep(GetLearningStepRequest request,
            StreamObserver<GetLearningStepResponse> responseObserver) {
        try {
            LearningStep step = learningStepRepository.findByIdCourseIdAndIdStepOrder(
                    request.getCourseId(), request.getStepNumber());

            if (step == null) {
                responseObserver.onError(
                        io.grpc.Status.NOT_FOUND.withDescription("Learning step not found").asRuntimeException());
                return;
            }

            responseObserver.onNext(GetLearningStepResponse.newBuilder()
                    .setLearningStep(convertToGrpcLearningStep(step))
                    .build());
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(io.grpc.Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
        }
    }

    @Override
    public void addLearningStep(AddLearningStepRequest request,
            StreamObserver<AddLearningStepResponse> responseObserver) {
        try {
            var grpcStep = request.getLearningStep();
            Course course = courseRepository.findById(grpcStep.getCourseId()).orElse(null);
            if (course == null) {
                responseObserver
                        .onError(io.grpc.Status.NOT_FOUND.withDescription("Course not found").asRuntimeException());
                return;
            }

            LearningStepType stepType = learningStepTypeRepository.findByName(grpcStep.getType());
            if (stepType == null) {
                responseObserver
                        .onError(io.grpc.Status.NOT_FOUND.withDescription("Type not found").asRuntimeException());
                return;
            }

            LearningStepId stepId = new LearningStepId(grpcStep.getStepOrder(), grpcStep.getCourseId());
            if (learningStepRepository.findByIdCourseIdAndIdStepOrder(grpcStep.getCourseId(),
                    grpcStep.getStepOrder()) != null) {
                responseObserver
                        .onError(io.grpc.Status.ALREADY_EXISTS.withDescription("Step exists").asRuntimeException());
                return;
            }

            LearningStep newStep = new LearningStep(stepId, course, stepType, grpcStep.getContent());
            newStep = learningStepRepository.save(newStep);

            responseObserver.onNext(AddLearningStepResponse.newBuilder()
                    .setLearningStep(convertToGrpcLearningStep(newStep))
                    .build());
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(io.grpc.Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
        }
    }

    @Override
    public void updateLearningStep(UpdateLearningStepRequest request,
            StreamObserver<UpdateLearningStepResponse> responseObserver) {
        try {
            var grpcStep = request.getLearningStep();
            LearningStep step = learningStepRepository.findByIdCourseIdAndIdStepOrder(
                    grpcStep.getCourseId(), grpcStep.getStepOrder());

            if (step == null) {
                responseObserver
                        .onError(io.grpc.Status.NOT_FOUND.withDescription("Step not found").asRuntimeException());
                return;
            }

            if (!grpcStep.getType().isEmpty()) {
                LearningStepType type = learningStepTypeRepository.findByName(grpcStep.getType());
                if (type != null)
                    step.setStepType(type);
            }
            step.setContent(grpcStep.getContent());
            learningStepRepository.save(step);

            responseObserver.onNext(UpdateLearningStepResponse.newBuilder()
                    .setLearningStep(convertToGrpcLearningStep(step))
                    .build());
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(io.grpc.Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
        }
    }

    private via.sep3.dataserver.grpc.Course convertToGrpcCourse(Course course) {
        int stepCount = learningStepRepository.countByIdCourseId(course.getId());

        var builder = via.sep3.dataserver.grpc.Course.newBuilder()
                .setId(course.getId())
                .setTitle(course.getTitle() != null ? course.getTitle() : "")
                .setDescription(course.getDescription() != null ? course.getDescription() : "")
                .setLanguage(course.getLanguage() != null ? course.getLanguage().getCode() : "")
                .setCategory(course.getCategory() != null ? course.getCategory().getName() : "")
                .setTotalSteps(stepCount);

        // map author
        if (course.getAuthor() != null) {
            builder.setAuthorId(course.getAuthor().getId());
        }

        // map approvedby
        if (course.getApprovedBy() != null) {
            builder.setApprovedBy(course.getApprovedBy().getId());
        } else {
            builder.setApprovedBy(0); 
        }

        return builder.build();
    }

    private via.sep3.dataserver.grpc.LearningStep convertToGrpcLearningStep(LearningStep step) {
        return via.sep3.dataserver.grpc.LearningStep.newBuilder()
                .setCourseId(step.getCourse().getId())
                .setContent(step.getContent())
                .setStepOrder(step.getId().getStepOrder())
                .setType(step.getStepType().getName())
                .build();
    }

    @Override
    public void createCategory(CreateCategoryRequest request,
            StreamObserver<CreateCategoryResponse> responseObserver) {
        try {
            CourseCategory category = new CourseCategory();
            category.setName(request.getName());
            category.setDescription(request.getDescription());

            category = courseCategoryRepository.save(category);

            CreateCategoryResponse response = CreateCategoryResponse.newBuilder()
                    .setCategory(convertToGrpcCategory(category))
                    .build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(io.grpc.Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
        }
    }

    @Override
    public void getCategories(GetCategoriesRequest request,
            StreamObserver<GetCategoriesResponse> responseObserver) {
        try {
            List<CourseCategory> categories = courseCategoryRepository.findAll();
            GetCategoriesResponse.Builder responseBuilder = GetCategoriesResponse.newBuilder();

            for (CourseCategory category : categories) {
                responseBuilder.addCategories(convertToGrpcCategory(category));
            }

            responseObserver.onNext(responseBuilder.build());
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(io.grpc.Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
        }
    }

    private via.sep3.dataserver.grpc.CourseCategory convertToGrpcCategory(CourseCategory category) {
        return via.sep3.dataserver.grpc.CourseCategory.newBuilder()
                .setId(category.getId())
                .setName(category.getName())
                .setDescription(category.getDescription() != null ? category.getDescription() : "")
                .build();
    }
}