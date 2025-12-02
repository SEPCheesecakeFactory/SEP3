package via.sep3.dataserver.data;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.Optional;

@Repository
public interface UserCourseProgressRepository extends JpaRepository<UserCourseProgress, UserCourseProgressId> {

    Optional<UserCourseProgress> findBySystemUser_IdAndCourse_Id(int systemUserId, int courseId);
}