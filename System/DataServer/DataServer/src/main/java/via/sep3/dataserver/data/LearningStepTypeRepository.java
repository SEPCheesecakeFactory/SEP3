package via.sep3.dataserver.data;

import org.springframework.data.jpa.repository.JpaRepository;

public interface LearningStepTypeRepository extends JpaRepository<LearningStepType, Integer>
{
  LearningStepType getLearningStepTypeById(int Id);
}
