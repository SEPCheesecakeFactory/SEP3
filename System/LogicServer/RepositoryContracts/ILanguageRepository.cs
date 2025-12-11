using BlazorApp.Entities; 
using Entities; 
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface ILanguageRepository : IRepositoryID<Language, CreateLanguageDto, Language, string>
    {
    }
}