package via.sep3.dataserver.data;

import jakarta.persistence.*;

@Entity
public class Course
{
  @Id
  @GeneratedValue(strategy = GenerationType.AUTO)
  private Integer id;

  @ManyToOne
  @JoinColumn(name = "language_code")
  private Language language;

  private String title;
  private String description;

  @ManyToOne
  @JoinColumn(name = "category_id")
  private CourseCategory category;

  public Language getLanguage()
  {
    return language;
  }

  public void setLanguage(Language language)
  {
    this.language = language;
  }

  public CourseCategory getCategory()
  {
    return category;
  }

  public void setCategory(CourseCategory category)
  {
    this.category = category;
  }

  public Integer getId()
  {
    return id;
  }

  public void setId(Integer id)
  {
    this.id = id;
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
}
