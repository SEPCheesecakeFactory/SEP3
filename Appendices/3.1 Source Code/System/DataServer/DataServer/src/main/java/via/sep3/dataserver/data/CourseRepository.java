package via.sep3.dataserver.data;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

public interface CourseRepository extends JpaRepository<Course, Integer>
{
  Course getCourseById(Integer id);
}
