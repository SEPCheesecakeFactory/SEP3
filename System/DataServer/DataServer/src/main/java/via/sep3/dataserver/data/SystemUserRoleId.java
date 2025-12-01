package via.sep3.dataserver.data;

import java.io.Serializable;
import java.util.Objects;

public class SystemUserRoleId implements Serializable {

  private int systemUser;
  private String role;

  public SystemUserRoleId() {}

  public SystemUserRoleId(int systemUser, String role) {
    this.systemUser = systemUser;
    this.role = role;
  }

  @Override
  public boolean equals(Object o) {
    if (this == o) return true;
    if (o == null || getClass() != o.getClass()) return false;
    SystemUserRoleId that = (SystemUserRoleId) o;
    return systemUser == that.systemUser && Objects.equals(role, that.role);
  }

  @Override
  public int hashCode() {
    return Objects.hash(systemUser, role);
  }
}