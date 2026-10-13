using JobTracker.Api.Dtos;
using JobTracker.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Api.Controllers;

[Route("api/job-applications/{applicationId:guid}/interviews")]
public class InterviewsController(IInterviewService service) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<InterviewResponse>>> GetAll(
        Guid applicationId, CancellationToken ct) =>
        ToActionResult(await service.GetAllAsync(applicationId, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InterviewResponse>> GetById(
        Guid applicationId, Guid id, CancellationToken ct) =>
        ToActionResult(await service.GetByIdAsync(applicationId, id, ct));

    [HttpPost]
    public async Task<ActionResult<InterviewResponse>> Create(
        Guid applicationId, CreateInterviewRequest req, CancellationToken ct)
    {
        var result = await service.CreateAsync(applicationId, req, ct);
        if (!result.IsSuccess) return ToError(result);

        return CreatedAtAction(nameof(GetById),
            new { applicationId, id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<InterviewResponse>> Update(
        Guid applicationId, Guid id, UpdateInterviewRequest req, CancellationToken ct) =>
        ToActionResult(await service.UpdateAsync(applicationId, id, req, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid applicationId, Guid id, CancellationToken ct)
    {
        var result = await service.DeleteAsync(applicationId, id, ct);
        if (!result.IsSuccess) return ToError(result);
        return NoContent();
    }
}