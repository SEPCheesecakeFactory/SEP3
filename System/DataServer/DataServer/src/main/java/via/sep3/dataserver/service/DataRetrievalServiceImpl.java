package via.sep3.dataserver.service;

import io.grpc.stub.StreamObserver;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import via.sep3.dataserver.data.Course;
import via.sep3.dataserver.data.CourseRepository;
import via.sep3.dataserver.grpc.DataRetrievalServiceGrpc;
import via.sep3.dataserver.grpc.GetCoursesRequest;
import via.sep3.dataserver.grpc.GetCoursesResponse;
import org.springframework.grpc.server.service.GrpcService;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

@GrpcService
@Service
public class DataRetrievalServiceImpl extends DataRetrievalServiceGrpc.DataRetrievalServiceImplBase {

    @Autowired
    private CourseRepository courseRepository;

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
}