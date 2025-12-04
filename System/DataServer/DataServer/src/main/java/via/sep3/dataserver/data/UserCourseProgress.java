package via.sep3.dataserver.data;

import jakarta.persistence.*;

@Entity
@Table(name = "user_course_progress", schema = "learn_db")
@IdClass(UserCourseProgressId.class)
public class UserCourseProgress {

    @Id
    @ManyToOne
    @JoinColumn(name = "user_id", referencedColumnName = "id")
    private SystemUser systemUser;

    @Id
    @ManyToOne
    @JoinColumn(name = "course_id", referencedColumnName = "id")
    private Course course;

    @Column(name = "current_step")
    private int currentStep;

    public UserCourseProgress() {}

    public UserCourseProgress(SystemUser systemUser, Course course, int currentStep) {
        this.systemUser = systemUser;
        this.course = course;
        this.currentStep = currentStep;
    }

    // Getters and Setters
    public SystemUser getSystemUser() {
        return systemUser;
    }

    public void setSystemUser(SystemUser systemUser) {
        this.systemUser = systemUser;
    }

    public Course getCourse() {
        return course;
    }

    public void setCourse(Course course) {
        this.course = course;
    }

    public int getCurrentStep() {
        return currentStep;
    }

    public void setCurrentStep(int currentStep) {
        this.currentStep = currentStep;
    }
}