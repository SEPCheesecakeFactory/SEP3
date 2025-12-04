package via.sep3.dataserver.data;

import org.springframework.data.jpa.repository.JpaRepository;

public interface CourseDraftRepository extends JpaRepository<CourseDraft, Integer>
{
  CourseDraft getCourseDraftById(Integer id);
}
