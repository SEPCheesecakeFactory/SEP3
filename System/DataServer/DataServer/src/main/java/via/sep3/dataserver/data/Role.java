package via.sep3.dataserver.data;

import jakarta.persistence.*;

@Entity
public class Role {

  @Id
  private String role;

  public String getRole() {
    return role;
  }

  public void setRole(String role) {
    this.role = role;
  }
}