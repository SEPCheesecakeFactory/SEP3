package via.sep3.dataserver.data;

import jakarta.persistence.*;

@Entity
@Table(name = "systemuserrole")
@IdClass(SystemUserRoleId.class) // Links to the key class created above
public class SystemUserRole {

  @Id
  @ManyToOne
  @JoinColumn(name = "systemuserid", referencedColumnName = "id")
  private SystemUser systemUser;

  @Id
  @ManyToOne
  @JoinColumn(name = "role", referencedColumnName = "role")
  private Role role;

  // Getters and Setters
  public SystemUser getSystemUser() {
    return systemUser;
  }

  public void setSystemUser(SystemUser systemUser) {
    this.systemUser = systemUser;
  }

  public Role getRole() {
    return role;
  }

  public void setRole(Role role) {
    this.role = role;
  }
}