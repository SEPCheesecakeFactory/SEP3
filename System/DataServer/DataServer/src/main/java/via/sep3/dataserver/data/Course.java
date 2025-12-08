package via.sep3.dataserver.data;

import jakarta.persistence.*;

@Entity
@Table(name = "course", schema = "learn_db")
public class Course {

  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  private Integer id;

  @ManyToOne
  @JoinColumn(name = "language")
  private Language language;

  private String title;

  @Column(length = 1000)
  private String description;

  @ManyToOne
  @JoinColumn(name = "category")
  private CourseCategory category;

  @ManyToOne
  @JoinColumn(name = "author_id", nullable = false)
  private SystemUser author;

  @ManyToOne
  @JoinColumn(name = "approved_by", nullable = true)
  private SystemUser approvedBy;

  public Integer getId() {
    return id;
  }

  public void setId(Integer id) {
    this.id = id;
  }

  public Language getLanguage() {
    return language;
  }

  public void setLanguage(Language language) {
    this.language = language;
  }

  public String getTitle() {
    return title;
  }

  public void setTitle(String title) {
    this.title = title;
  }

  public String getDescription() {
    return description;
  }

  public void setDescription(String description) {
    this.description = description;
  }

  public CourseCategory getCategory() {
    return category;
  }

  public void setCategory(CourseCategory category) {
    this.category = category;
  }

  public SystemUser getAuthor() {
    return author;
  }

  public void setAuthor(SystemUser author) {
    this.author = author;
  }

  public SystemUser getApprovedBy() {
    return approvedBy;
  }

  public void setApprovedBy(SystemUser approvedBy) {
    this.approvedBy = approvedBy;
  }
}