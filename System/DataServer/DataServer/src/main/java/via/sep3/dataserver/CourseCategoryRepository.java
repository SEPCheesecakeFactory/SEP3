package via.sep3.dataserver;

import org.springframework.data.jpa.repository.JpaRepository;

public interface CourseCategoryRepository extends JpaRepository<CourseCategory, Integer>
{
  CourseCategory getCourseCategoryById(Integer id);
}
