using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace BlazorApp.Services;

public class CrudController<T> : ControllerBase where T : class
{
    private readonly IRepository<T> _repository;

    public CrudController(IRepository<T> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<ActionResult<T>> Add(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ActionResult<T>> GetAsync(int id)
    {
        var addedEntity = await _repository.GetSingleAsync(id);
        if (addedEntity == null)
            return NotFound();

        return Ok(addedEntity);
    }

    public ActionResult<IEnumerable<T>> GetAllMany()
    {
        var entities = _repository.GetMany();

        var entitiesDtos = entities
            .Select(ls => new learningStepDto
            {
                Id = ls.Id,
                Type = ls.Type,
                Content = ls.Content,
                CourseId = ls.CourseId
            })
            .ToList();

        return Ok(entitiesDtos);
    }

    public async Task DeleteAsync(string endpoint, int? id = null)
    {
        throw new NotImplementedException();
    }

    public async Task<T> UpdateAsync<T>(int id, [FromBody] T request)
    {
        throw new NotImplementedException();
    }
}