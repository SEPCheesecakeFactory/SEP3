using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RESTAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GenericController<MainType, ID>(IRepositoryID<MainType, ID> repository) : ControllerBase where MainType : class
    {
        private readonly IRepositoryID<MainType, ID> _repository = repository;
        protected virtual Func<string, ID> IdParser { get; } = idStr => (ID)Convert.ChangeType(idStr, typeof(ID));

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            _repository.GetMany(); // Dummy call to avoid unused field warning
            return Ok(new { status = $"Service is running" });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MainType>> GetSingleAsync(string id)
        {
            ID parsedId = IdParser(id);
            try
            {
                var entity = await _repository.GetSingleAsync(parsedId);
                return Ok(entity);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<MainType>> GetMany()
        {
            var entities = _repository.GetMany();
            return Ok(entities);
        }

        [HttpPost]
        public async Task<ActionResult<MainType>> CreateAsync([FromBody] MainType entity)
        {
            var createdEntity = await _repository.AddAsync(entity);
            return CreatedAtAction(nameof(GetSingleAsync), new { id = createdEntity }, createdEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] MainType entity)
        {
            ID parsedId = IdParser(id);
            try
            {
                await _repository.UpdateAsync(entity);
                return Ok(_repository.GetSingleAsync(parsedId).Result);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            ID parsedId = IdParser(id);
            try
            {
                await _repository.DeleteAsync(parsedId);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
