package via.sep3.dataserver.data;

import org.springframework.data.jpa.repository.JpaRepository;

public interface CourseRepository extends JpaRepository<Course, Integer>
{
  Course getCourseById(Integer id);
}
