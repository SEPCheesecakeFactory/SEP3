package via.sep3.dataserver.data;

import jakarta.persistence.*; // Imports everything needed
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "systemuser")
public class SystemUser
{
  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  @Column(name = "id")
  private Integer id;

  @Column(name = "username")
  private String username;

  @Column(name = "password_hash")
  private String password_hash;

  @OneToMany(mappedBy = "systemUser", fetch = FetchType.EAGER, cascade = CascadeType.ALL)
  private List<SystemUserRole> systemUserRoles = new ArrayList<>();

  public Integer getId()
  {
    return id;
  }

  public void setId(Integer id)
  {
    this.id = id;
  }

  public String getUsername()
  {
    return username;
  }

  public void setUsername(String username)
  {
    this.username = username;
  }

  public String getPassword()
  {
    return password_hash;
  }

  public void setPassword(String password_hash)
  {
    this.password_hash = password_hash;
  }

  public List<SystemUserRole> getSystemUserRoles() {
    return systemUserRoles;
  }

  public void setSystemUserRoles(List<SystemUserRole> systemUserRoles) {
    this.systemUserRoles = systemUserRoles;
  }
}