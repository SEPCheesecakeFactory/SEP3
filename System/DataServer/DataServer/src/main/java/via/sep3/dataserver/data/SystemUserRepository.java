package via.sep3.dataserver.data;

import org.springframework.data.jpa.repository.JpaRepository;

public interface SystemUserRepository extends JpaRepository<SystemUser, Integer>
{
  SystemUser getSystemUserById(Integer id);
}
