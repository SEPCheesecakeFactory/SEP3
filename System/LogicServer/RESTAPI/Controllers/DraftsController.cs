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
    public async Task<ActionResult<Draft>> ApproveDraft([FromBody] int approvedBy ,[FromRoute] int draftId)
    {
        try
        {
            course_repository.
            Draft currentDraft = await _repository.GetSingleAsync(draftId);
            Draft updatedDraft = new Draft{
                Id=currentDraft.Id,
                Language=currentDraft.Language,
                Title=currentDraft.Title,
                Description=currentDraft.Description,
                TeacherId=currentDraft.TeacherId,
                CourseId=courseId,
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
