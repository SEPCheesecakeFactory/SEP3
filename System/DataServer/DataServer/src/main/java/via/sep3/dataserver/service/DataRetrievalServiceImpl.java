package via.sep3.dataserver.service;

import java.util.ArrayList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.grpc.server.service.GrpcService;
import org.springframework.stereotype.Service;

import io.grpc.stub.StreamObserver;
import via.sep3.dataserver.data.*;
import via.sep3.dataserver.data.Course;
import via.sep3.dataserver.data.LearningStep;
import via.sep3.dataserver.data.LearningStepRepository;
import via.sep3.dataserver.data.LearningStepType;
import via.sep3.dataserver.data.LearningStepTypeRepository;
import via.sep3.dataserver.data.Role;
import via.sep3.dataserver.data.SystemUser;
import via.sep3.dataserver.grpc.*;
import via.sep3.dataserver.grpc.CourseDraft;

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
  private LearningStepTypeRepository learningStepTypeRepository;

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


      courseDraftRepository.save(courseDraft);

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
        .setTeacherId(courseDraft.getSystemUser().getId())
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

    return builder.build();
  }
}