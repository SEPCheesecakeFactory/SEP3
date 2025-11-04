package via.sep3.dataserver.data;

import org.springframework.data.jpa.repository.JpaRepository;

public interface LanguageRepository extends JpaRepository<Language, String>
{
  Language getLanguageByCode(String code);
}
