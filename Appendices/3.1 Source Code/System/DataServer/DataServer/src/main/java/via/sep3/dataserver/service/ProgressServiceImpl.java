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
import via.sep3.dataserver.data.LeaderboardEntry;
import via.sep3.dataserver.data.Course;
import via.sep3.dataserver.data.*;
import via.sep3.dataserver.grpc.*;

@GrpcService
@Service
public class ProgressServiceImpl extends ProgressServiceGrpc.ProgressServiceImplBase {

    @Autowired
    private UserCourseProgressRepository progressRepository;
    @Autowired
    private SystemUserRepository userRepository;
    @Autowired
    private CourseRepository courseRepository;

    @Override
    public void getCourseProgress(CourseProgressRequest request,
            StreamObserver<CourseProgressResponse> responseObserver) {
        try {
            Optional<UserCourseProgress> progressEntity = progressRepository
                    .findBySystemUser_IdAndCourse_Id(request.getUserId(), request.getCourseId());

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
            responseObserver.onError(io.grpc.Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
        }
    }

    @Override
    public void updateCourseProgress(CourseProgressUpdate request, StreamObserver<Empty> responseObserver) {
        try {
            Optional<UserCourseProgress> existingProgress = progressRepository
                    .findBySystemUser_IdAndCourse_Id(request.getUserId(), request.getCourseId());

            UserCourseProgress progressToSave;

            if (existingProgress.isPresent()) {
                progressToSave = existingProgress.get();
                progressToSave.setCurrentStep(request.getCurrentStep());
            } else {
                SystemUser user = userRepository.findById(request.getUserId())
                        .orElseThrow(() -> new RuntimeException("User not found: " + request.getUserId()));
                Course course = courseRepository.findById(request.getCourseId())
                        .orElseThrow(() -> new RuntimeException("Course not found: " + request.getCourseId()));

                progressToSave = new UserCourseProgress(user, course, request.getCurrentStep());
            }

            progressRepository.save(progressToSave);

            responseObserver.onNext(Empty.newBuilder().build());
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(io.grpc.Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
        }
    }

    @Override
    public void getLeaderboard(Empty request, StreamObserver<GetLeaderboardResponse> responseObserver) {
        try {
            // ✅ FIX: Increase the limit to 1000 (or larger) to get all active students.
            // This ensures the Client receives the full list and can calculate neighbors.
            Pageable allPlayers = PageRequest.of(0, 1000);

            // Update the variable name in the query call too
            List<LeaderboardEntry> dbEntries = userRepository.findTopPlayers(allPlayers);

            List<via.sep3.dataserver.grpc.LeaderboardEntry> grpcEntries = new ArrayList<>();
            int rank = 1;

            for (LeaderboardEntry dbEntry : dbEntries) {
                grpcEntries.add(via.sep3.dataserver.grpc.LeaderboardEntry.newBuilder()
                        .setUsername(dbEntry.getUsername())
                        .setTotalScore((int) dbEntry.getTotalScore())
                        .setRank(rank++)
                        .build());
            }

            GetLeaderboardResponse response = GetLeaderboardResponse.newBuilder()
                    .addAllEntries(grpcEntries)
                    .build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();
        } catch (Exception e) {
            responseObserver.onError(io.grpc.Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
        }
    }
}