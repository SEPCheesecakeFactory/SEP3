package via.sep3.dataserver.data;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.Query;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;

public interface SystemUserRepository extends JpaRepository<SystemUser, Integer>
{
  SystemUser getSystemUserById(Integer id);

  @Query("SELECT u.username as username, SUM(p.currentStep * 10) as totalScore " +
           "FROM SystemUser u JOIN UserCourseProgress p ON u.id = p.systemUser.id " +
           "GROUP BY u.username " +
           "ORDER BY totalScore DESC")
    List<LeaderboardEntry> findTopPlayers(Pageable pageable);
}
