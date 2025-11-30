package via.sep3.dataserver.data;

import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface LearningStepRepository extends JpaRepository<LearningStep, LearningStepId>
{
  List<LearningStep> findByIdCourseIdOrderByIdStepOrder(Integer courseId);

  LearningStep findByIdCourseIdAndIdStepOrder(Integer courseId, Integer stepOrder);
  
  List<LearningStep> findByCourse_TitleOrderByIdStepOrder(String courseTitle);
}
