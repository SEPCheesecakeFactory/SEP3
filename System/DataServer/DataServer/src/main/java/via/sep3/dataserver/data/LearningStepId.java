package via.sep3.dataserver.data;

import java.io.Serializable;
import java.util.Objects;
import jakarta.persistence.*;

@Embeddable
public class LearningStepId implements Serializable {
  @Column(name = "step_order", nullable = false)
  private Integer stepOrder;

  @Column(name = "course_id", nullable = false)
  private Integer courseId;

  public LearningStepId() {}
  public LearningStepId(Integer stepOrder, Integer courseId) {
    this.stepOrder = stepOrder; this.courseId = courseId;
  }

  public Integer getStepOrder() { return stepOrder; }
  public Integer getCourseId()  { return courseId; }

  @Override public boolean equals(Object o) {
    if (this == o) return true;
    if (!(o instanceof LearningStepId that)) return false;
    return Objects.equals(stepOrder, that.stepOrder)
        && Objects.equals(courseId, that.courseId);
  }
  @Override public int hashCode() { return Objects.hash(stepOrder, courseId); }
}
