using JobTracker.Api.Dtos;
using JobTracker.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Api.Controllers;

[Route("api/job-applications")]
public class JobApplicationsController(IJobApplicationService service) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<JobApplicationResponse>>> Search(
        [FromQuery] JobApplicationQuery query, CancellationToken ct) =>
        Ok(await service.SearchAsync(query, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobApplicationResponse>> GetById(Guid id, CancellationToken ct) =>
        ToActionResult(await service.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<JobApplicationResponse>> Create(
        CreateJobApplicationRequest req, CancellationToken ct)
    {
        var result = await service.CreateAsync(req, ct);
        if (!result.IsSuccess) return ToError(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<JobApplicationResponse>> Update(
        Guid id, UpdateJobApplicationRequest req, CancellationToken ct) =>
        ToActionResult(await service.UpdateAsync(id, req, ct));

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<JobApplicationResponse>> ChangeStatus(
        Guid id, ChangeStatusRequest req, CancellationToken ct) =>
        ToActionResult(await service.ChangeStatusAsync(id, req, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await service.DeleteAsync(id, ct);
        if (!result.IsSuccess) return ToError(result);
        return NoContent();
    }
}