using JobTracker.Api.Dtos;
using JobTracker.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Api.Controllers;

[Route("api/job-applications/{applicationId:guid}/notes")]
public class NotesController(INoteService service) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<NoteResponse>>> GetAll(
        Guid applicationId, CancellationToken ct) =>
        ToActionResult(await service.GetAllAsync(applicationId, ct));

    [HttpPost]
    public async Task<ActionResult<NoteResponse>> Create(
        Guid applicationId, CreateNoteRequest req, CancellationToken ct)
    {
        var result = await service.CreateAsync(applicationId, req, ct);
        if (!result.IsSuccess) return ToError(result);

        return CreatedAtAction(nameof(GetAll), new { applicationId }, result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid applicationId, Guid id, CancellationToken ct)
    {
        var result = await service.DeleteAsync(applicationId, id, ct);
        if (!result.IsSuccess) return ToError(result);
        return NoContent();
    }
}