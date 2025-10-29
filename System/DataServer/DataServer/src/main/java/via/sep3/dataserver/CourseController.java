package via.sep3.dataserver;

import jdk.jfr.Category;
import org.springframework.web.bind.annotation.*;

@RestController
public class CourseController
{
  private final CourseRepository courseRepository;
  private final CourseCategoryRepository courseCategoryRepository;
  private final LanguageRepository languageRepository;

  public CourseController(CourseRepository courseRepository,
      CourseCategoryRepository courseCategoryRepository,
      LanguageRepository languageRepository)
  {
    this.courseRepository = courseRepository;
    this.courseCategoryRepository = courseCategoryRepository;
    this.languageRepository = languageRepository;
  }

  @GetMapping("/courses/{id}")
  public Course findCourseById(@PathVariable Integer id)
  {
    return courseRepository.getCourseById(id);
  }

  @GetMapping("/categories/{id}")
  public CourseCategory findCourseCategoryById(@PathVariable Integer id)
  {
    return courseCategoryRepository.getCourseCategoryById(id);
  }

  @GetMapping("/languages/{code}")
  public Language findLanguageByCode(@PathVariable String code)
  {
    return languageRepository.getLanguageByCode(code);
  }

  @PostMapping("/courses/add")
  public String addCourse(@RequestBody Course course)
  {
    courseRepository.save(course);
    return"Course added";
  }

  @PostMapping("/categories/add")
  public String addCategory(@RequestBody CourseCategory category)
  {
    courseCategoryRepository.save(category);
    return"Category added";
  }

  @PostMapping("/languages/add")
  public String addLanguage(@RequestBody Language language)
  {
    languageRepository.save(language);
    return"Language added";
  }



}
