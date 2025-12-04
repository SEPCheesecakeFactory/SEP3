package via.sep3.dataserver.service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.grpc.server.service.GrpcService;
import org.springframework.stereotype.Service;

import io.grpc.stub.StreamObserver;
import via.sep3.dataserver.data.Course;
import via.sep3.dataserver.data.CourseCategory;
import via.sep3.dataserver.data.CourseCategoryRepository;
import via.sep3.dataserver.data.CourseDraftRepository;
import via.sep3.dataserver.data.CourseRepository;
import via.sep3.dataserver.data.Language;
import via.sep3.dataserver.data.LanguageRepository;
import via.sep3.dataserver.data.LearningStep;
import via.sep3.dataserver.data.LearningStepRepository;
import via.sep3.dataserver.data.LearningStepType;
import via.sep3.dataserver.data.LearningStepTypeRepository;
import via.sep3.dataserver.data.Role;
import via.sep3.dataserver.data.RoleRepository;
import via.sep3.dataserver.data.SystemUser;
import via.sep3.dataserver.data.SystemUserRepository;
import via.sep3.dataserver.data.SystemUserRole;
import via.sep3.dataserver.data.SystemUserRoleRepository;
import via.sep3.dataserver.data.UserCourseProgressRepository;
import via.sep3.dataserver.grpc.AddCourseRequest;
import via.sep3.dataserver.grpc.AddCourseResponse;
import via.sep3.dataserver.grpc.AddDraftRequest;
import via.sep3.dataserver.grpc.AddDraftResponse;
import via.sep3.dataserver.grpc.AddUserRequest;
import via.sep3.dataserver.grpc.AddUserResponse;
import via.sep3.dataserver.grpc.CourseDraft;
import via.sep3.dataserver.grpc.CourseProgressRequest;
import via.sep3.dataserver.grpc.CourseProgressResponse;
import via.sep3.dataserver.grpc.CourseProgressUpdate;
import via.sep3.dataserver.grpc.DataRetrievalServiceGrpc;
import via.sep3.dataserver.grpc.Empty;
import via.sep3.dataserver.grpc.GetCoursesRequest;
import via.sep3.dataserver.grpc.GetCoursesResponse;
import via.sep3.dataserver.grpc.GetDraftRequest;
import via.sep3.dataserver.grpc.GetDraftResponse;
import via.sep3.dataserver.grpc.GetDraftsRequest;
import via.sep3.dataserver.grpc.GetDraftsResponse;
import via.sep3.dataserver.grpc.GetLeaderboardResponse;
import via.sep3.dataserver.grpc.GetLearningStepResponse;
import via.sep3.dataserver.grpc.GetUsersRequest;
import via.sep3.dataserver.grpc.GetUsersResponse;
import via.sep3.dataserver.grpc.UpdateCourseRequest;
import via.sep3.dataserver.grpc.UpdateCourseResponse;
import via.sep3.dataserver.grpc.UpdateDraftRequest;
import via.sep3.dataserver.grpc.UpdateDraftResponse;

@GrpcService
@Service
public class DataRetrievalServiceImpl extends DataRetrievalServiceGrpc.DataRetrievalServiceImplBase {

  @Autowired
  private CourseRepository courseRepository;
  @Autowired
  private SystemUserRepository userRepository;
  @Autowired
  private SystemUserRoleRepository systemUserRoleRepository;
  @Autowired
  private RoleRepository roleRepository;
  @Autowired
  private LearningStepRepository learningStepRepository;
  
  @Autowired
  private CourseDraftRepository courseDraftRepository;
  @Autowired
  private UserCourseProgressRepository progressRepository;

  @Autowired
  private LearningStepTypeRepository learningStepTypeRepository;

  @Autowired
  private LanguageRepository languageRepository;

  @Autowired
  private CourseCategoryRepository courseCategoryRepository;

  @Override
  public void getCourses(GetCoursesRequest request, StreamObserver<GetCoursesResponse> responseObserver) {
    try {
      List<via.sep3.dataserver.data.Course> courses = courseRepository.findAll();

      GetCoursesResponse.Builder responseBuilder = GetCoursesResponse.newBuilder();

      for (via.sep3.dataserver.data.Course course : courses) {
        
        int stepCount = learningStepRepository.countByIdCourseId(course.getId());

        via.sep3.dataserver.grpc.Course grpcCourse = via.sep3.dataserver.grpc.Course.newBuilder()
            .setId(course.getId())
            .setTitle(course.getTitle())
            .setDescription(course.getDescription())
            .setLanguage(course.getLanguage() != null ? course.getLanguage().getName() : "") 
            .setCategory(course.getCategory() != null ? course.getCategory().getName() : "")
            .setTotalSteps(stepCount)
            .build();

        responseBuilder.addCourses(grpcCourse);
      }

      responseObserver.onNext(responseBuilder.build());
      responseObserver.onCompleted();

    } catch (Exception e) {
      System.out.println("Error in getCourses: " + e.getMessage());
      e.printStackTrace();
      responseObserver.onError(io.grpc.Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
    }
  }

  @Override
  public void addCourse(AddCourseRequest request, StreamObserver<AddCourseResponse> responseObserver) {
    try {
      via.sep3.dataserver.data.Language language = languageRepository.findById(request.getLanguage()).orElse(null);
      if (language == null) {
         responseObserver.onError(io.grpc.Status.NOT_FOUND.withDescription("Language not found: " + request.getLanguage()).asRuntimeException());
         return;
      }
      
      via.sep3.dataserver.data.CourseCategory category = courseCategoryRepository.findByName(request.getCategory());
       if (category == null) {
         responseObserver.onError(io.grpc.Status.NOT_FOUND.withDescription("Category not found: " + request.getCategory()).asRuntimeException());
         return;
      }

      Course course = new Course();
      course.setTitle(request.getTitle());
      course.setDescription(request.getDescription());
      course.setLanguage(language);
      course.setCategory(category);

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

  private via.sep3.dataserver.grpc.Course convertToGrpcCourse(Course course) {
    return via.sep3.dataserver.grpc.Course.newBuilder()
        .setId(course.getId())
        .setTitle(course.getTitle() != null ? course.getTitle() : "")
        .setDescription(course.getDescription() != null ? course.getDescription() : "")
        .setLanguage(course.getLanguage() != null ? course.getLanguage().getCode() : "")
        .setCategory(course.getCategory() != null ? course.getCategory().getName() : "")
        .build();
  }

  @Override
  public void getLearningStep(via.sep3.dataserver.grpc.GetLearningStepRequest request,
      StreamObserver<via.sep3.dataserver.grpc.GetLearningStepResponse> responseObserver) {
    try {
      // custom query method instead of findById to avoid composite key issues
      LearningStep step = learningStepRepository.findByIdCourseIdAndIdStepOrder(
          request.getCourseId(),
          request.getStepNumber());

      if (step == null) {
        responseObserver.onError(
            io.grpc.Status.NOT_FOUND
                .withDescription("Learning step not found for courseId=" + request.getCourseId()
                    + ", stepNumber=" + request.getStepNumber())
                .asRuntimeException());
        return;
      }

      responseObserver.onNext(GetLearningStepResponse.newBuilder()
          .setLearningStep(convertToGrpcLearningStep(step))
          .build());
      responseObserver.onCompleted();
    } catch (Exception e) {
      responseObserver.onError(
          io.grpc.Status.INTERNAL
              .withDescription("Error retrieving learning step: " + e.getMessage())
              .withCause(e)
              .asRuntimeException());
    }
  }

  @Override
  public void getUsers(GetUsersRequest request, StreamObserver<GetUsersResponse> responseObserver) {
    try {
      List<SystemUser> users = userRepository.findAll();
      List<via.sep3.dataserver.grpc.User> grpcUsers = new ArrayList<>();

      for (SystemUser user : users) {
        via.sep3.dataserver.grpc.User grpcUser = convertToGrpcUser(user);
        grpcUsers.add(grpcUser);
      }
      GetUsersResponse response = GetUsersResponse.newBuilder()
          .addAllUsers(grpcUsers)
          .build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();
    } catch (Exception e) {
      responseObserver.onError(e);
    }
  }

  private via.sep3.dataserver.grpc.User convertToGrpcUser(SystemUser jpaUser) {
    via.sep3.dataserver.grpc.User.Builder userBuilder = via.sep3.dataserver.grpc.User.newBuilder()
        .setId(jpaUser.getId())
        .setUsername(jpaUser.getUsername() != null ? jpaUser.getUsername() : "")
        .setPassword(jpaUser.getPassword() != null ? jpaUser.getPassword() : "");

    if (jpaUser.getSystemUserRoles() != null) {
      for (via.sep3.dataserver.data.SystemUserRole junction : jpaUser.getSystemUserRoles()) {
        via.sep3.dataserver.data.Role dbRole = junction.getRole();

        if (dbRole != null) {
          userBuilder.addRoles(
              via.sep3.dataserver.grpc.Role.newBuilder()
                  .setRole(dbRole.getRole())
                  .build());
        }
      }
    }

    return userBuilder.build();
  }

  @Override
  public void addUser(AddUserRequest request,
      StreamObserver<AddUserResponse> responseObserver) {
    try {
      String username = request.getUsername();
      String password = request.getPassword();
      List<String> roleStrings = request.getRolesList();
      List<Role> assignedRoles = new ArrayList<>();

      SystemUser newUser = new SystemUser();
      newUser.setUsername(username);
      newUser.setPassword(password);

      userRepository.save(newUser);

      for (String roleName : roleStrings) {
        via.sep3.dataserver.data.Role roleEntity = roleRepository.findById(roleName)
            .orElseGet(() -> {
              via.sep3.dataserver.data.Role r = new via.sep3.dataserver.data.Role();
              r.setRole(roleName);
              return roleRepository.save(r);
            });

        SystemUserRole junction = new SystemUserRole();
        junction.setSystemUser(newUser);
        junction.setRole(roleEntity);

        systemUserRoleRepository.save(junction);

        assignedRoles.add(roleEntity);

        via.sep3.dataserver.grpc.User grpcUser = convertToGrpcUser(newUser);

        AddUserResponse response = AddUserResponse.newBuilder()
            .setUser(grpcUser)
            .build();

        responseObserver.onNext(response);
        responseObserver.onCompleted();
      }
    } catch (Exception e) {
      responseObserver.onError(e);
    }
  }

@Override
public void updateCourse(UpdateCourseRequest request,
                         StreamObserver<UpdateCourseResponse> responseObserver) {
    try {
        var grpcCourse = request.getCourse();

        // 1. Load the existing course
        Course course = courseRepository.findById(grpcCourse.getId())
                .orElse(null);

        if (course == null) {
            responseObserver.onError(
                io.grpc.Status.NOT_FOUND
                    .withDescription("Course not found: id=" + grpcCourse.getId())
                    .asRuntimeException());
            return;
        }

        // 2. Update simple fields
        course.setTitle(grpcCourse.getTitle());
        course.setDescription(grpcCourse.getDescription());

        // 3. Convert language string → entity
        Language lang = languageRepository.findById(grpcCourse.getLanguage())
                .orElse(null);

        if (lang == null) {
            responseObserver.onError(
                io.grpc.Status.NOT_FOUND
                    .withDescription("Language not found: " + grpcCourse.getLanguage())
                    .asRuntimeException());
            return;
        }

        course.setLanguage(lang);

        // 4. Convert category string → entity
        CourseCategory cat = courseCategoryRepository.findByName(grpcCourse.getCategory());

        if (cat == null) {
            responseObserver.onError(
                io.grpc.Status.NOT_FOUND
                    .withDescription("Category not found: " + grpcCourse.getCategory())
                    .asRuntimeException());
            return;
        }

        course.setCategory(cat);

        // 5. Save
        courseRepository.save(course);

        // 6. Convert back to gRPC response
        var grpcUpdated = via.sep3.dataserver.grpc.Course.newBuilder()
            .setId(course.getId())
            .setTitle(course.getTitle())
            .setDescription(course.getDescription())
            .setLanguage(course.getLanguage().getCode())
            .setCategory(course.getCategory().getName())
            .build();

        var response = UpdateCourseResponse.newBuilder()
            .setCourse(grpcUpdated)
            .build();

        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }
    catch (Exception e) {
        responseObserver.onError(
            io.grpc.Status.INTERNAL
                .withDescription("Error updating course: " + e.getMessage())
                .asRuntimeException());
    }
}




  @Override
  public void updateLearningStep(via.sep3.dataserver.grpc.UpdateLearningStepRequest request,
      StreamObserver<via.sep3.dataserver.grpc.UpdateLearningStepResponse> responseObserver) {
    try {
      via.sep3.dataserver.grpc.LearningStep grpcStep = request.getLearningStep();

      LearningStep step = learningStepRepository.findByIdCourseIdAndIdStepOrder(
          grpcStep.getCourseId(),
          grpcStep.getStepOrder());

      if (step == null) {
        responseObserver.onError(
            io.grpc.Status.NOT_FOUND
                .withDescription("Learning step not found for courseId=" + grpcStep.getCourseId()
                    + ", stepOrder=" + grpcStep.getStepOrder())
                .asRuntimeException());
        return;
      }

      // if step type null or empty, keep existing type
      if (grpcStep.getType() != null && !grpcStep.getType().isEmpty()) {
        via.sep3.dataserver.data.LearningStepType stepType = learningStepTypeRepository
            .findByName(grpcStep.getType());
        if (stepType == null) {
          responseObserver.onError(
              io.grpc.Status.NOT_FOUND
                  .withDescription("Learning step type not found: " + grpcStep.getType())
                  .asRuntimeException());
          return;
        }
        step.setStepType(stepType);
      }

      LearningStepType newStepType;
      try
      {
        newStepType = learningStepTypeRepository
          .findByName(grpcStep.getType());
      }
      catch (Exception e)
      {
        responseObserver.onError(
          io.grpc.Status.NOT_FOUND
            .withDescription("Learning step type not found: " + grpcStep.getType())
            .asRuntimeException());
        return;
      }
      step.setStepType(newStepType);
      step.setContent(grpcStep.getContent());      
      learningStepRepository.save(step);

      responseObserver.onNext(via.sep3.dataserver.grpc.UpdateLearningStepResponse.newBuilder()
          .setLearningStep(convertToGrpcLearningStep(step))
          .build());
      responseObserver.onCompleted();
    } catch (Exception e) {
      responseObserver.onError(
          io.grpc.Status.INTERNAL
              .withDescription("Error updating learning step: " + e.getMessage())
              .withCause(e)
              .asRuntimeException());
    }
  }

  private via.sep3.dataserver.grpc.LearningStep convertToGrpcLearningStep(LearningStep step) {
    return via.sep3.dataserver.grpc.LearningStep.newBuilder()
        .setCourseId(step.getCourse().getId())
        .setContent(step.getContent())
        .setStepOrder(step.getId().getStepOrder())
        .setType(step.getStepType().getName())
        .build();
  }

  //drafts

  @Override public void addDraft(AddDraftRequest request,
      StreamObserver<AddDraftResponse> responseObserver)
  {
    try
    {
      String language = request.getLanguage();
      String title = request.getTitle();
      String description = request.getDescription();
      int teacher_id = request.getTeacherId();
      SystemUser systemUser = userRepository.getSystemUserById(teacher_id);


      via.sep3.dataserver.data.CourseDraft courseDraft = new via.sep3.dataserver.data.CourseDraft();
      courseDraft.setLanguage(language);
      courseDraft.setTitle(title);
      courseDraft.setDescription(description);
      courseDraft.setSystemUser(systemUser);


      courseDraft = courseDraftRepository.save(courseDraft);

      CourseDraft grpcCourseDraft = convertToGrpcDraft(courseDraft);

      AddDraftResponse response = AddDraftResponse.newBuilder()
          .setCourseDraft(grpcCourseDraft)
          .build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();

    }
    catch (Exception e)
    {
      responseObserver.onError(e);
    }
  }

  @Override public void getDraft(GetDraftRequest request,
      StreamObserver<GetDraftResponse> responseObserver)
  {
    try{
      via.sep3.dataserver.data.CourseDraft courseDraft = courseDraftRepository.getCourseDraftById(request.getDraftId());
      CourseDraft grpcCourseDraft = convertToGrpcDraft(courseDraft);
      GetDraftResponse response = GetDraftResponse.newBuilder()
          .setCourseDraft(grpcCourseDraft)
          .build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
   catch (Exception e) {
    responseObserver.onError(e);
  }

  }
  private CourseDraft convertToGrpcDraft(via.sep3.dataserver.data.CourseDraft courseDraft)
  {
    CourseDraft.Builder builder = CourseDraft.newBuilder()
        .setId(courseDraft.getId())
        .setLanguage(courseDraft.getLanguage() != null ? courseDraft.getLanguage() : "")
        .setTitle(courseDraft.getTitle() != null ? courseDraft.getTitle() : "")
        .setDescription(courseDraft.getDescription() != null ? courseDraft.getDescription() : "");

    if (courseDraft.getCourse() != null) {
      builder.setCourseId(courseDraft.getCourse().getId());
    } else {
      builder.setCourseId(-1);
    }
    if (courseDraft.getSystemUser() != null) {
      builder.setTeacherId(courseDraft.getSystemUser().getId());
    } else {
      builder.setTeacherId(-1);
    }
    if (courseDraft.getApprovedBy() != null) {
      builder.setApprovedBy(courseDraft.getApprovedBy().getId());
    } else {
      builder.setApprovedBy(-1);
    }

    return builder.build();
  }

  @Override public void getDrafts(GetDraftsRequest request,
      StreamObserver<GetDraftsResponse> responseObserver)
  {
    try {
      List<via.sep3.dataserver.data.CourseDraft> drafts = courseDraftRepository.findAll();
      GetDraftsResponse.Builder responseBuilder = GetDraftsResponse.newBuilder();

      for (via.sep3.dataserver.data.CourseDraft draft : drafts) {
        responseBuilder.addDrafts(convertToGrpcDraft(draft));
      }

      responseObserver.onNext(responseBuilder.build());
      responseObserver.onCompleted();
    } catch (Exception e) {
      responseObserver.onError(e);
    }
  }

  @Override public void updateDraft(UpdateDraftRequest request,
      StreamObserver<UpdateDraftResponse> responseObserver)
  {
    try
    {
      via.sep3.dataserver.grpc.CourseDraft grpcDraft = request.getCourseDraft();

      via.sep3.dataserver.data.CourseDraft existingDraft = courseDraftRepository.findById(grpcDraft.getId()).orElse(null);

      if (existingDraft == null) {
        responseObserver.onError(io.grpc.Status.NOT_FOUND.withDescription("Draft not found").asRuntimeException());
        return;
      }

      if (!grpcDraft.getLanguage().isEmpty()) {
        existingDraft.setLanguage(grpcDraft.getLanguage());
      }
      if (!grpcDraft.getTitle().isEmpty()) {
        existingDraft.setTitle(grpcDraft.getTitle());
      }
      if (!grpcDraft.getDescription().isEmpty()) {
        existingDraft.setDescription(grpcDraft.getDescription());
      }

      if (grpcDraft.getTeacherId() != -1 && (existingDraft.getSystemUser() == null || existingDraft.getSystemUser().getId() != grpcDraft.getTeacherId())) {
        SystemUser teacher = userRepository.findById(grpcDraft.getTeacherId()).orElse(null);
        if (teacher != null) {
          existingDraft.setSystemUser(teacher);
        }
      }

      if (grpcDraft.getCourseId() != -1) {
        if (existingDraft.getCourse() == null || existingDraft.getCourse().getId() != grpcDraft.getCourseId()) {
          Course course = courseRepository.findById(grpcDraft.getCourseId()).orElse(null);
          if (course != null) {
            existingDraft.setCourse(course);
          }
        }
      }

      if (grpcDraft.getApprovedBy() != -1) {
        if (existingDraft.getApprovedBy() == null || existingDraft.getApprovedBy().getId() != grpcDraft.getApprovedBy()) {
          SystemUser approver = userRepository.findById(grpcDraft.getApprovedBy()).orElse(null);
          if (approver != null) {
            existingDraft.setApprovedBy(approver);
          }
        }
      }

      existingDraft = courseDraftRepository.save(existingDraft);

      CourseDraft grpcCourseDraft = convertToGrpcDraft(existingDraft);

      UpdateDraftResponse response = UpdateDraftResponse.newBuilder()
          .setCourseDraft(grpcCourseDraft)
          .build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();

    }
    catch (Exception e)
    {
      responseObserver.onError(e);
    }
  }
@Override
    public void getCourseProgress(CourseProgressRequest request, StreamObserver<CourseProgressResponse> responseObserver) {
        try {
            Optional<via.sep3.dataserver.data.UserCourseProgress> progressEntity = progressRepository.findBySystemUser_IdAndCourse_Id(
                    request.getUserId(),
                    request.getCourseId());

            int step = 1;

            if (progressEntity.isPresent()) {
                step = progressEntity.get().getCurrentStep();
            }

            CourseProgressResponse response = CourseProgressResponse.newBuilder()
                    .setCurrentStep(step)
                    .build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();

        } catch (Exception e) {
            System.out.println("Error in getCourseProgress: " + e.getMessage());
            e.printStackTrace();
            responseObserver.onError(io.grpc.Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
        }
    }

    @Override
    public void updateCourseProgress(CourseProgressUpdate request, StreamObserver<Empty> responseObserver) {
        try {
            Optional<via.sep3.dataserver.data.UserCourseProgress> existingProgress = progressRepository.findBySystemUser_IdAndCourse_Id(
                    request.getUserId(),
                    request.getCourseId());

            via.sep3.dataserver.data.UserCourseProgress progressToSave;

            if (existingProgress.isPresent()) {
                progressToSave = existingProgress.get();
                progressToSave.setCurrentStep(request.getCurrentStep());
            } else {
                via.sep3.dataserver.data.SystemUser user = userRepository.findById(request.getUserId())
                        .orElseThrow(() -> new RuntimeException("User not found: " + request.getUserId()));
                via.sep3.dataserver.data.Course course = courseRepository.findById(request.getCourseId())
                        .orElseThrow(() -> new RuntimeException("Course not found: " + request.getCourseId()));

                progressToSave = new via.sep3.dataserver.data.UserCourseProgress(user, course, request.getCurrentStep());
            }

            progressRepository.save(progressToSave);

            responseObserver.onNext(Empty.newBuilder().build());
            responseObserver.onCompleted();

        } catch (Exception e) {
            System.out.println("Error in updateCourseProgress: " + e.getMessage());
            e.printStackTrace();
            responseObserver.onError(io.grpc.Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
        }
    }

@Override
  public void getLeaderboard(Empty request, StreamObserver<GetLeaderboardResponse> responseObserver) {
    try {
      // create a pagerequest to get the top 10 results
      Pageable topTen = PageRequest.of(0, 10);

      // call the custom repo method
      List<via.sep3.dataserver.data.LeaderboardEntry> dbEntries = userRepository.findTopPlayers(topTen);

      // map db entries to proto messages
      List<via.sep3.dataserver.grpc.LeaderboardEntry> grpcEntries = new ArrayList<>();
      int rank = 1;

      for (via.sep3.dataserver.data.LeaderboardEntry dbEntry : dbEntries) {
        grpcEntries.add(via.sep3.dataserver.grpc.LeaderboardEntry.newBuilder()
            .setUsername(dbEntry.getUsername())
            .setTotalScore((int) dbEntry.getTotalScore()) 
            .setRank(rank++) // cncrement rank 1, 2, 3...
            .build());
      }

      // build and send response
      GetLeaderboardResponse response = GetLeaderboardResponse.newBuilder()
          .addAllEntries(grpcEntries)
          .build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();

    } catch (Exception e) {
      System.out.println("Error fetching leaderboard: " + e.getMessage());
      e.printStackTrace();
      responseObserver.onError(io.grpc.Status.INTERNAL
          .withDescription("Error fetching leaderboard: " + e.getMessage())
          .asRuntimeException());
    }
  }

}