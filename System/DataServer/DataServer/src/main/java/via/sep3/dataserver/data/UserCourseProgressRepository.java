package via.sep3.dataserver.data;

import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

@Repository
public interface UserCourseProgressRepository extends JpaRepository<UserCourseProgress, UserCourseProgressId> {

    Optional<UserCourseProgress> findBySystemUser_IdAndCourse_Id(int systemUserId, int courseId);

    List<UserCourseProgress> findBySystemUser_Id(int id);

    @Transactional
    void deleteBySystemUser_IdAndCourse_Id(int systemUserId, int courseId);

    @Modifying
    @Transactional
    @Query("DELETE FROM UserCourseProgress u WHERE u.systemUser.id = :userId AND u.course.id = :courseId")
    void deleteUserCourseProgress(@Param("courseId") int courseId, @Param("userId") int userId);
}