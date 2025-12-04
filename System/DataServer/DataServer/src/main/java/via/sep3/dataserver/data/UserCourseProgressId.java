package via.sep3.dataserver.data;

import java.io.Serializable;
import java.util.Objects;

public class UserCourseProgressId implements Serializable {

    private int systemUser; 
    private int course; 

    public UserCourseProgressId() {}

    public UserCourseProgressId(int systemUser, int course) {
        this.systemUser = systemUser;
        this.course = course;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        UserCourseProgressId that = (UserCourseProgressId) o;
        return systemUser == that.systemUser && course == that.course;
    }

    @Override
    public int hashCode() {
        return Objects.hash(systemUser, course);
    }
}