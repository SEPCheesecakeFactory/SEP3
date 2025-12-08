using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RESTAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IRepositoryID<User, User, User, int> repository) : GenericDefaultController<User, User, User, int>(repository)
{
    
}
