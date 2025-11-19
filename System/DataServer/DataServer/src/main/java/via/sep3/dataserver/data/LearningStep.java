package via.sep3.dataserver.data;

import jakarta.persistence.Column;
import jakarta.persistence.EmbeddedId;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.MapsId;
import jakarta.persistence.Table;

@Entity
@Table(name = "learningstep", schema = "learn_db")
public class LearningStep {
  @EmbeddedId
  private LearningStepId id;

  @MapsId("courseId")
  @ManyToOne(fetch = FetchType.EAGER, optional = false)
  @JoinColumn(name = "course_id", nullable = false)
  private Course course;

  @ManyToOne(fetch = FetchType.EAGER, optional = false)
  @JoinColumn(name = "step_type", nullable = false)
  private LearningStepType stepType;

  @Column(name = "content")
  private String content;

  public LearningStep() {
  }

  public LearningStep(LearningStepId id, Course course,
      LearningStepType stepType, String content) {
    this.id = id;
    this.course = course;
    this.stepType = stepType;
    this.content = content;
  }

  public LearningStepId getId() {
    return id;
  }

  public void setId(LearningStepId id) {
    this.id = id;
  }

  public Course getCourse() {
    return course;
  }

  public void setCourse(Course course) {
    this.course = course;
  }

  public LearningStepType getStepType() {
    return stepType;
  }

  public void setStepType(LearningStepType stepType) {
    this.stepType = stepType;
  }

  public String getContent() {
    return content;
  }

  public void setContent(String content) {
    this.content = content;
  }
}