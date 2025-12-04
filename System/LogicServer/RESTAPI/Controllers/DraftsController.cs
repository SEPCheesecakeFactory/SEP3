using Entities;
using RepositoryContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RESTAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class DraftsController(IRepositoryID<Draft, CreateDraftDto, Draft, int> repository, IRepositoryID<Course, CreateCourseDto, Course, int> course_repository ) : GenericController<Draft, CreateDraftDto, Draft, int>(repository)
{
    [HttpGet]
    [Authorize("MustBeAdmin")]
    public ActionResult<IEnumerable<Draft>> HttpGetMany() => GetMany();

    [HttpPost, Authorize("MustBeTeacher")]
    public async Task<ActionResult<Draft>> AddDraft([FromBody] CreateDraftDto dto)
    {
        try
        {
            var created = await _repository.AddAsync(dto);
            return Created($"/drafts/{created.Id}", created);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    [HttpPut("{id:int}"), Authorize("MustBeAdmin")]
    public async Task<ActionResult<Draft>> ApproveDraft([FromBody] int approvedBy,[FromRoute] int id)
    {
        try
        {
            Draft currentDraft = await _repository.GetSingleAsync(id);
            CreateCourseDto courseDto = new CreateCourseDto{
                Language = currentDraft.Language,
                Title = currentDraft.Title,
                Description = currentDraft.Description,
                Category = "default"
            };
            Course newCourse = await course_repository.AddAsync(courseDto);
            Draft updatedDraft = new Draft{
                Id=currentDraft.Id,
                Language=currentDraft.Language,
                Title=currentDraft.Title,
                Description=currentDraft.Description,
                TeacherId=currentDraft.TeacherId,
                CourseId=newCourse.Id,
                ApprovedBy=approvedBy
            };

            currentDraft = await _repository.UpdateAsync(updatedDraft);
            return Ok(updatedDraft);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Draft>> GetDraft(int id) => await GetSingleAsync(id.ToString());
}
