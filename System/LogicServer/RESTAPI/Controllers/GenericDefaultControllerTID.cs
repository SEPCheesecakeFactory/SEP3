using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RESTAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class GenericDefaultController<MainType, ID>(IRepositoryID<MainType, MainType, MainType, ID> repository) : GenericDefaultController<MainType, MainType, MainType, ID>(repository) where MainType : class
{
}
