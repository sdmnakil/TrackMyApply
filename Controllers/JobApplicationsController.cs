using System.Security.Claims;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobApplicationsController : ControllerBase
{
    private readonly IJobApplicationService _service;

    public JobApplicationsController(
        IJobApplicationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<JobApplicationDto>>> GetAll(
        int page = 1,
        int pageSize = 10,
        string? sortBy = "applicationDate",
        string? sortOrder = "desc")
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest(
                "Page and pageSize must be greater than 0.");
        }

        var userId = GetUserId();

        var result = await _service.GetAllAsync(
            userId,
            page,
            pageSize,
            sortBy,
            sortOrder);

        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<JobApplicationDto>>> Search(
        string? company,
        string? status,
        string? sortBy = "applicationDate",
        string? sortOrder = "desc")
    {
        var userId = GetUserId();

        var applications = await _service.SearchAsync(
            userId,
            company,
            status,
            sortBy,
            sortOrder);

        return Ok(applications);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobApplicationDto>> GetById(
        int id)
    {
        var userId = GetUserId();

        var application =
            await _service.GetByIdAsync(id, userId);

        if (application == null)
        {
            return NotFound();
        }

        return Ok(application);
    }

    [HttpPost]
    public async Task<ActionResult<JobApplicationDto>> Create(
        CreateJobApplicationDto dto)
    {
        var userId = GetUserId();

        var createdApplication =
            await _service.AddAsync(userId, dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdApplication.Id },
            createdApplication);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateJobApplicationDto dto)
    {
        var userId = GetUserId();

        var updated =
            await _service.UpdateAsync(
                id,
                userId,
                dto);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();

        var deleted =
            await _service.DeleteAsync(id, userId);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    private int GetUserId()
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        return int.Parse(userId!);
    }
}