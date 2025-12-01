package via.sep3.dataserver.data;

import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface SystemUserRoleRepository extends JpaRepository<SystemUserRole, SystemUserRoleId> {

  List<SystemUserRole> findBySystemUser(SystemUser user);
}
