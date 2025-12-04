package via.sep3.dataserver.data;

import jakarta.persistence.*;

@Entity
public class CourseDraft
{
  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  private Integer id;

  private String language;
  private String title;
  private String description;

  @ManyToOne
  @JoinColumn(name = "teacher_id")
  private SystemUser systemUser;

  @ManyToOne
  @JoinColumn(name = "course_id")
  private Course course;

  @ManyToOne
  @JoinColumn(name="approved_by")
  private SystemUser approvedBy;


  public Integer getId()
  {
    return id;
  }

  public void setId(Integer id)
  {
    this.id = id;
  }

  public String getLanguage()
  {
    return language;
  }

  public void setLanguage(String language)
  {
    this.language = language;
  }

  public String getTitle()
  {
    return title;
  }

  public void setTitle(String title)
  {
    this.title = title;
  }

  public String getDescription()
  {
    return description;
  }

  public void setDescription(String description)
  {
    this.description = description;
  }

  public SystemUser getSystemUser()
  {
    return systemUser;
  }

  public void setSystemUser(SystemUser systemUser)
  {
    this.systemUser = systemUser;
  }

  public Course getCourse()
  {
    return course;
  }

  public void setCourse(Course course)
  {
    this.course = course;
  }

  public SystemUser getApprovedBy()
  {
    return approvedBy;
  }

  public void setApprovedBy(SystemUser approvedBy)
  {
    this.approvedBy = approvedBy;
  }
}
