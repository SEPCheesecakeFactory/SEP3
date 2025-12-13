using System;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using Microsoft.AspNetCore.Authorization;

namespace RESTAPI.Controllers;

[Route("[controller]")]
public class LearningStepsController(IRepositoryID<LearningStep, LearningStep, LearningStep, (int, int)> repository) : GenericController<LearningStep, LearningStep, LearningStep, (int, int)>(repository)
{
    protected override Func<string, (int, int)> IdParser { get; } = idStr =>
    {
        var parts = idStr.Split('_');
        if (parts.Length != 2)
        {
            throw new ArgumentException("Invalid ID format. Expected format: 'courseId_stepOrder'");
        }
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    };

    [HttpGet("{id}")]
    public async Task<ActionResult<LearningStep>> HttpGetSingleAsync(string id) => await GetSingleAsync(id);

    [HttpGet]
    [Authorize("MustBeTeacherOrAdmin")] // Not necessarily in the requirements, but the least for now
    public ActionResult<IEnumerable<LearningStep>> HttpGetMany() => GetMany();

    [HttpPost]
    [Authorize("MustBeTeacher")]
    public async Task<ActionResult<LearningStep>> HttpCreateAsync([FromBody] LearningStep entity) => await CreateAsync(entity);

    [HttpPut, HttpPut("{id}")]
    [Authorize("MustBeTeacher")]
    public async Task<ActionResult<LearningStep>> HttpUpdateAsync([FromBody] LearningStep entity) => await UpdateAsync(entity);

    [HttpDelete("{id}")]
    [Authorize("MustBeTeacher")]
    public async Task<IActionResult> HttpDeleteAsync(string id) => await DeleteAsync(id);
}
