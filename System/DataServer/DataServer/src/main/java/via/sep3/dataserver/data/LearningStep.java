package via.sep3.dataserver.data;

import jakarta.persistence.*;

@Entity
public class LearningStep {
  @EmbeddedId
  private LearningStepId id;

  @MapsId("courseId")
  @ManyToOne(fetch = FetchType.LAZY, optional = false)
  @JoinColumn(name = "course_id", nullable = false)
  private Course course;

  @ManyToOne(fetch = FetchType.LAZY, optional = false)
  @JoinColumn(name = "step_type", nullable = false)
  private LearningStepType stepType;

  @Column(name = "content")
  private String content;

  public LearningStep() {}
  public LearningStep(LearningStepId id, Course course,
                      LearningStepType stepType, String content) {
    this.id = id; this.course = course; this.stepType = stepType; this.content = content;
  }

  public LearningStepId getId() { return id; }
  public void setId(LearningStepId id) { this.id = id; }

  public Course getCourse() { return course; }
  public void setCourse(Course course) { this.course = course; }

  public LearningStepType getStepType() { return stepType; }
  public void setStepType(LearningStepType stepType) { this.stepType = stepType; }

  public String getContent() { return content; }
  public void setContent(String content) { this.content = content; }
}
