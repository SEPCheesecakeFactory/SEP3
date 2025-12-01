package via.sep3.dataserver.service;

import io.grpc.stub.StreamObserver;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import via.sep3.dataserver.data.*;
import via.sep3.dataserver.data.Course;
import via.sep3.dataserver.data.Role;
import via.sep3.dataserver.data.SystemUser;
import via.sep3.dataserver.grpc.*;
import org.springframework.grpc.server.service.GrpcService;

import java.util.ArrayList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.grpc.server.service.GrpcService;
import org.springframework.stereotype.Service;

import io.grpc.stub.StreamObserver;
import via.sep3.dataserver.data.Course;
import via.sep3.dataserver.data.CourseRepository;
import via.sep3.dataserver.data.LearningStep;
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
    private SystemUserRepository userRepository;
    @Autowired
    private SystemUserRoleRepository systemUserRoleRepository;
    @Autowired
    private RoleRepository roleRepository;

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
    try {
      // custom query method instead of findById to avoid composite key issues
      LearningStep step = learningStepRepository.findByIdCourseIdAndIdStepOrder(
          request.getCourseId(), 
          request.getStepNumber()
      );
      
      if (step == null) {
        responseObserver.onError(
            io.grpc.Status.NOT_FOUND
                .withDescription("Learning step not found for courseId=" + request.getCourseId() 
                    + ", stepNumber=" + request.getStepNumber())
                .asRuntimeException()
        );
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
              .asRuntimeException()
      );
    }

  @Override public void getUsers(GetUsersRequest request, StreamObserver<GetUsersResponse> responseObserver)
  {
    try {
      List<SystemUser> users = userRepository.findAll();
      List<via.sep3.dataserver.grpc.SystemUser> grpcUsers = new ArrayList<>();

      for (SystemUser user : users) {
        via.sep3.dataserver.grpc.SystemUser grpcUser = convertToGrpcUser(user);
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

  private via.sep3.dataserver.grpc.SystemUser convertToGrpcUser(SystemUser jpaUser) {
    via.sep3.dataserver.grpc.SystemUser.Builder userBuilder =
        via.sep3.dataserver.grpc.SystemUser.newBuilder()
            .setId(jpaUser.getId())
            .setUsername(jpaUser.getUsername() != null ? jpaUser.getUsername() : "")
            .setPasswordHash(jpaUser.getPassword() != null ? jpaUser.getPassword() : "");

    // iterate over the list attached to the user object.

    if (jpaUser.getSystemUserRoles() != null) {
      for (via.sep3.dataserver.data.SystemUserRole junction : jpaUser.getSystemUserRoles()) {

        // 1. Extract the Role Entity from the Junction Table
        via.sep3.dataserver.data.Role dbRole = junction.getRole();

        if (dbRole != null) {
          via.sep3.dataserver.grpc.Role grpcRole =
              via.sep3.dataserver.grpc.Role.newBuilder()
                  .setRole(dbRole.getRole()) // This gets the string "admin", "learner", etc.
                  .build();

          userBuilder.addRoles(grpcRole);
        }
      }
    }

    return userBuilder.build();
  }

  @Override public void addUser(AddUserRequest request,
      StreamObserver<AddUserResponse> responseObserver)
  {
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

        via.sep3.dataserver.grpc.SystemUser grpcUser = convertToGrpcUser(newUser);

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