package via.sep3.dataserver.service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.grpc.server.service.GrpcService;
import org.springframework.stereotype.Service;

import io.grpc.stub.StreamObserver;
import via.sep3.dataserver.data.Course;
import via.sep3.dataserver.data.CourseRepository;
import via.sep3.dataserver.data.LearningStep;
import via.sep3.dataserver.data.LearningStepRepository;
import via.sep3.dataserver.data.Role;
import via.sep3.dataserver.data.RoleRepository;
import via.sep3.dataserver.data.SystemUser;
import via.sep3.dataserver.data.SystemUserRepository;
import via.sep3.dataserver.data.SystemUserRole;
import via.sep3.dataserver.data.SystemUserRoleRepository;
import via.sep3.dataserver.data.UserCourseProgress;
import via.sep3.dataserver.data.UserCourseProgressRepository;
import via.sep3.dataserver.grpc.AddUserRequest;
import via.sep3.dataserver.grpc.AddUserResponse;
import via.sep3.dataserver.grpc.CourseProgressRequest;
import via.sep3.dataserver.grpc.CourseProgressResponse;
import via.sep3.dataserver.grpc.CourseProgressUpdate;
import via.sep3.dataserver.grpc.DataRetrievalServiceGrpc;
import via.sep3.dataserver.grpc.Empty;
import via.sep3.dataserver.grpc.GetCoursesRequest;
import via.sep3.dataserver.grpc.GetCoursesResponse;
import via.sep3.dataserver.grpc.GetLearningStepResponse;
import via.sep3.dataserver.grpc.GetUsersRequest;
import via.sep3.dataserver.grpc.GetUsersResponse;

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
  private UserCourseProgressRepository progressRepository;

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

  private via.sep3.dataserver.grpc.LearningStep convertToGrpcLearningStep(LearningStep step) {
    return via.sep3.dataserver.grpc.LearningStep.newBuilder()
        .setCourseId(step.getCourse().getId())
        .setContent(step.getContent())
        .setStepOrder(step.getId().getStepOrder())
        .setType(step.getStepType().getName())
        .build();
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



}