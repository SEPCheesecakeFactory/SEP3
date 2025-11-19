package via.sep3.dataserver.service;

import java.util.ArrayList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.grpc.server.service.GrpcService;
import org.springframework.stereotype.Service;

import io.grpc.stub.StreamObserver;
import via.sep3.dataserver.data.Course;
import via.sep3.dataserver.data.CourseRepository;
import via.sep3.dataserver.data.LearningStep;
import via.sep3.dataserver.data.LearningStepId;
import via.sep3.dataserver.data.LearningStepRepository;
import via.sep3.dataserver.grpc.DataRetrievalServiceGrpc;
import via.sep3.dataserver.grpc.GetCoursesRequest;
import via.sep3.dataserver.grpc.GetCoursesResponse;
import via.sep3.dataserver.grpc.GetLearningStepResponse;

@GrpcService
@Service
public class DataRetrievalServiceImpl extends DataRetrievalServiceGrpc.DataRetrievalServiceImplBase {

  @Autowired
  private CourseRepository courseRepository;

  @Autowired
  private LearningStepRepository learningStepRepository;

  @Override
  public void getCourses(GetCoursesRequest request, StreamObserver<GetCoursesResponse> responseObserver) {
    try {
      List<Course> courses = courseRepository.findAll();
      List<via.sep3.dataserver.grpc.Course> grpcCourses = new ArrayList<>();

      for (Course course : courses) {
        via.sep3.dataserver.grpc.Course grpcCourse = convertToGrpcCourse(course);
        grpcCourses.add(grpcCourse);
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
    var a = learningStepRepository.findById(
        new LearningStepId(request.getStepNumber(), request.getCourseId()));
    if (a.isEmpty()) {
      responseObserver.onError(new RuntimeException("Learning step not found"));
      return;
    }
    LearningStep step = a.get();
    responseObserver.onNext(GetLearningStepResponse.newBuilder()
        .setLearningStep(convertToGrpcLearningStep(step))
        .build());
    responseObserver.onCompleted();
  }

  private via.sep3.dataserver.grpc.LearningStep convertToGrpcLearningStep(LearningStep step) {
    return via.sep3.dataserver.grpc.LearningStep.newBuilder()
        .setCourseId(step.getCourse().getId())
        .setContent(step.getContent())
        .setStepOrder(step.getId().getStepOrder())
        .setType(step.getStepType().getName())
        .build();
  }
}