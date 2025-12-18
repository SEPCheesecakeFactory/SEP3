package via.sep3.dataserver.service;

import java.util.ArrayList;
import java.util.List;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.grpc.server.service.GrpcService;
import org.springframework.stereotype.Service;
import io.grpc.stub.StreamObserver;
import via.sep3.dataserver.data.Role;
import via.sep3.dataserver.data.SystemUser;
import via.sep3.dataserver.data.SystemUserRepository;
import via.sep3.dataserver.data.SystemUserRole;
import via.sep3.dataserver.data.SystemUserRoleRepository;
import via.sep3.dataserver.data.RoleRepository;
import via.sep3.dataserver.grpc.AddUserRequest;
import via.sep3.dataserver.grpc.AddUserResponse;
import via.sep3.dataserver.grpc.GetUserRequest;
import via.sep3.dataserver.grpc.GetUsersRequest;
import via.sep3.dataserver.grpc.GetUsersResponse;
import via.sep3.dataserver.grpc.UpdateUserRequest;
import via.sep3.dataserver.grpc.UserServiceGrpc;

@GrpcService
@Service
public class UserServiceImpl extends UserServiceGrpc.UserServiceImplBase {

    @Autowired
    private SystemUserRepository userRepository;
    @Autowired
    private SystemUserRoleRepository systemUserRoleRepository;
    @Autowired
    private RoleRepository roleRepository;

    @Override
    public void getUsers(GetUsersRequest request, StreamObserver<GetUsersResponse> responseObserver) {
        try {
            List<SystemUser> users = userRepository.findAll();
            List<via.sep3.dataserver.grpc.User> grpcUsers = new ArrayList<>();

            for (SystemUser user : users) {
                grpcUsers.add(convertToGrpcUser(user));
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

    @Override
    public void getUser(GetUserRequest request, StreamObserver<via.sep3.dataserver.grpc.User> responseObserver) {
        try {
            SystemUser user = userRepository.findById(request.getId())
                    .orElseThrow(() -> new RuntimeException("User not found"));

            responseObserver.onNext(convertToGrpcUser(user));
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(e);
        }
    }

    @Override
    public void addUser(AddUserRequest request, StreamObserver<AddUserResponse> responseObserver) {
        try {
            String username = request.getUsername();
            String password = request.getPassword();
            List<String> roleStrings = request.getRolesList();

            // create new User
            SystemUser newUser = new SystemUser();
            newUser.setUsername(username);
            newUser.setPassword(password);

            userRepository.save(newUser);

            // assign Roles
            for (String roleName : roleStrings) {
                Role roleEntity = roleRepository.findById(roleName)
                        .orElseGet(() -> {
                            Role r = new Role();
                            r.setRole(roleName);
                            return roleRepository.save(r);
                        });

                SystemUserRole junction = new SystemUserRole();
                junction.setSystemUser(newUser);
                junction.setRole(roleEntity);
                systemUserRoleRepository.save(junction);
            }

            AddUserResponse response = AddUserResponse.newBuilder()
                    .setUser(convertToGrpcUser(newUser))
                    .build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(e);
        }
    }

    @Override
    public void updateUser(UpdateUserRequest request, StreamObserver<via.sep3.dataserver.grpc.User> responseObserver) {
        try {
            int id = request.getId();
            SystemUser user = userRepository.findById(id).orElseThrow(() -> new RuntimeException("User not found"));

            if (!request.getUsername().isEmpty()) {
                user.setUsername(request.getUsername());
            }
            if (!request.getPassword().isEmpty()) {
                user.setPassword(request.getPassword());
            }

            // Clear existing roles - orphanRemoval=true in SystemUser will delete them from DB
            user.getSystemUserRoles().clear();

            for (String roleName : request.getRolesList()) {
                Role roleEntity = roleRepository.findById(roleName)
                        .orElseGet(() -> {
                            Role r = new Role();
                            r.setRole(roleName);
                            return roleRepository.save(r);
                        });

                SystemUserRole junction = new SystemUserRole();
                junction.setSystemUser(user);
                junction.setRole(roleEntity);
                // No need to save junction explicitly, CascadeType.ALL will handle it
                user.getSystemUserRoles().add(junction);
            }
            
            userRepository.save(user);

            responseObserver.onNext(convertToGrpcUser(user));
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(e);
        }
    }

    private via.sep3.dataserver.grpc.User convertToGrpcUser(SystemUser jpaUser) {
        var userBuilder = via.sep3.dataserver.grpc.User.newBuilder()
                .setId(jpaUser.getId())
                .setUsername(jpaUser.getUsername() != null ? jpaUser.getUsername() : "")
                .setPassword(jpaUser.getPassword() != null ? jpaUser.getPassword() : "");

        if (jpaUser.getSystemUserRoles() != null) {
            for (SystemUserRole junction : jpaUser.getSystemUserRoles()) {
                Role dbRole = junction.getRole();
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
}