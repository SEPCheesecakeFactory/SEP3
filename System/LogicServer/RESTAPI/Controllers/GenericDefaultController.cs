using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RESTAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class GenericDefaultController<MainType, AddType, UpdateType, ID>(IRepositoryID<MainType, AddType, UpdateType, ID> repository) : GenericController<MainType, AddType, UpdateType, ID>(repository) where MainType : class
{
    [HttpGet("status")]
    public IActionResult HttpGetStatus() => GetStatus();

    [HttpGet("{id}")]
    public async Task<ActionResult<MainType>> HttpGetSingleAsync(string id) => await GetSingleAsync(id);

    [HttpGet]
    public ActionResult<IEnumerable<MainType>> HttpGetMany() => GetMany();

    [HttpPost]
    public async Task<ActionResult<MainType>> HttpCreateAsync([FromBody] AddType entity) => await CreateAsync(entity);

    [HttpPut("{id}")]
    public async Task<IActionResult> HttpUpdateAsync(string id, [FromBody] UpdateType entity) => await UpdateAsync(id, entity);

    [HttpDelete("{id}")]
    public async Task<IActionResult> HttpDeleteAsync(string id) => await DeleteAsync(id);
}
