package via.sep3.dataserver.service;

import io.grpc.stub.StreamObserver;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.grpc.server.service.GrpcService;
import via.sep3.dataserver.data.*;
import via.sep3.dataserver.grpc.DataRetrievalServiceGrpc;
import via.sep3.dataserver.grpc.Empty;
import via.sep3.dataserver.grpc.GetCoursesRequest;
import via.sep3.dataserver.grpc.GetCoursesResponse;
import via.sep3.dataserver.grpc.GetLearningStepsRequest;
import via.sep3.dataserver.grpc.GetLearningStepsResponse;
import via.sep3.dataserver.grpc.LearningStepKey;

import java.util.ArrayList;
import java.util.List;

@GrpcService
public class DataRetrievalServiceImpl extends DataRetrievalServiceGrpc.DataRetrievalServiceImplBase {

    @Autowired
    private CourseRepository courseRepository;

    @Autowired
    private LearningStepRepository learningStepRepository;

    @Autowired
    private LearningStepTypeRepository learningStepTypeRepository;

    // --- 1. GET COURSES ---
    @Override
    public void getCourses(GetCoursesRequest request, StreamObserver<GetCoursesResponse> responseObserver) {
        try {
            List<Course> courses = courseRepository.findAll();
            List<via.sep3.dataserver.grpc.Course> grpcCourses = new ArrayList<>();

            for (Course course : courses) {
                grpcCourses.add(mapCourseToProto(course));
            }
            GetCoursesResponse response = GetCoursesResponse.newBuilder()
                    .addAllCourses(grpcCourses)
                    .build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(e);
        }
    }

    // --- 2. GET LEARNING STEPS (Many) ---
    @Override
    public void getLearningSteps(GetLearningStepsRequest request, StreamObserver<GetLearningStepsResponse> responseObserver) {
        try {
            // Use the custom method to find by Course ID
            List<LearningStep> steps = learningStepRepository.findByIdCourseIdOrderByIdStepOrder(request.getCourseId());
            
            var responseBuilder = GetLearningStepsResponse.newBuilder();
            for (LearningStep step : steps) {
                responseBuilder.addLearningSteps(mapStepToProto(step));
            }

            responseObserver.onNext(responseBuilder.build());
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(e);
        }
    }

    // --- 3. GET LEARNING STEP (Single) ---
    @Override
    public void getLearningStep(LearningStepKey request, StreamObserver<via.sep3.dataserver.grpc.LearningStep> responseObserver) {
        try {
            // Construct Composite Key
            LearningStepId id = new LearningStepId(request.getStepOrder(), request.getCourseId());
            
            var step = learningStepRepository.findById(id);
            
            if (step.isPresent()) {
                responseObserver.onNext(mapStepToProto(step.get()));
                responseObserver.onCompleted();
            } else {
                responseObserver.onError(io.grpc.Status.NOT_FOUND
                    .withDescription("Learning step not found").asRuntimeException());
            }
        } catch (Exception e) {
            responseObserver.onError(e);
        }
    }

    // --- 4. ADD LEARNING STEP ---
    @Override
    public void addLearningStep(via.sep3.dataserver.grpc.LearningStep request, StreamObserver<via.sep3.dataserver.grpc.LearningStep> responseObserver) {
        try {
            // A. Verify Course exists
            Course course = courseRepository.findById(request.getCourseId())
                .orElseThrow(() -> new RuntimeException("Course not found"));

            // B. Find the Type entity (e.g. "Video" -> ID 2)
            LearningStepType typeEntity = learningStepTypeRepository.findAll().stream()
                .filter(t -> t.getName().equalsIgnoreCase(request.getType()))
                .findFirst()
                .orElseThrow(() -> new RuntimeException("Invalid Step Type: " + request.getType()));

            // C. Calculate next Order
            // (Count existing steps for this course + 1)
            var existingSteps = learningStepRepository.findByIdCourseIdOrderByIdStepOrder(request.getCourseId());
            int nextOrder = existingSteps.size() + 1;

            // D. Create ID and Entity
            LearningStepId id = new LearningStepId(nextOrder, request.getCourseId());
            LearningStep entity = new LearningStep(id, course, typeEntity, request.getContent());

            // E. Save
            LearningStep saved = learningStepRepository.save(entity);

            responseObserver.onNext(mapStepToProto(saved));
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(e);
        }
    }

    // --- 5. UPDATE LEARNING STEP ---
    @Override
    public void updateLearningStep(via.sep3.dataserver.grpc.LearningStep request, StreamObserver<via.sep3.dataserver.grpc.LearningStep> responseObserver) {
        try {
            LearningStepId id = new LearningStepId(request.getStepOrder(), request.getCourseId());
            
            LearningStep existingStep = learningStepRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Step not found"));

            // Update content
            existingStep.setContent(request.getContent());

            // Update type if changed
            if (!existingStep.getStepType().getName().equalsIgnoreCase(request.getType())) {
                 LearningStepType newType = learningStepTypeRepository.findAll().stream()
                    .filter(t -> t.getName().equalsIgnoreCase(request.getType()))
                    .findFirst()
                    .orElseThrow(() -> new RuntimeException("Invalid Step Type"));
                 existingStep.setStepType(newType);
            }

            LearningStep updated = learningStepRepository.save(existingStep);
            
            responseObserver.onNext(mapStepToProto(updated));
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(e);
        }
    }

    // --- 6. DELETE LEARNING STEP ---
    @Override
    public void deleteLearningStep(LearningStepKey request, StreamObserver<Empty> responseObserver) {
        try {
            LearningStepId id = new LearningStepId(request.getStepOrder(), request.getCourseId());
            
            if(learningStepRepository.existsById(id)){
                learningStepRepository.deleteById(id);
                responseObserver.onNext(Empty.newBuilder().build());
                responseObserver.onCompleted();
            } else {
                 responseObserver.onError(io.grpc.Status.NOT_FOUND
                    .withDescription("Step not found").asRuntimeException());
            }
        } catch (Exception e) {
            responseObserver.onError(e);
        }
    }

    // --- HELPER MAPPERS ---

    private via.sep3.dataserver.grpc.Course mapCourseToProto(Course course) {
        return via.sep3.dataserver.grpc.Course.newBuilder()
                .setId(course.getId())
                .setTitle(course.getTitle() != null ? course.getTitle() : "")
                .setDescription(course.getDescription() != null ? course.getDescription() : "")
                .setLanguage(course.getLanguage() != null ? course.getLanguage().getCode() : "")
                .setCategory(course.getCategory() != null ? course.getCategory().getName() : "")
                .build();
    }

    private via.sep3.dataserver.grpc.LearningStep mapStepToProto(LearningStep step) {
        return via.sep3.dataserver.grpc.LearningStep.newBuilder()
                .setCourseId(step.getId().getCourseId())
                .setStepOrder(step.getId().getStepOrder())
                .setContent(step.getContent())
                .setType(step.getStepType().getName()) // Returns string "Video", "Text", etc.
                .build();
    }
}