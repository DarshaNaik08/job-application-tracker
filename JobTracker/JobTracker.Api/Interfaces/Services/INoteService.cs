using JobTracker.Api.Common;
using JobTracker.Api.Dtos;

namespace JobTracker.Api.Interfaces.Services;

public interface INoteService
{
    Task<ServiceResult<List<NoteResponse>>> GetAllAsync(Guid applicationId, CancellationToken ct);
    Task<ServiceResult<NoteResponse>> CreateAsync(Guid applicationId, CreateNoteRequest req, CancellationToken ct);
    Task<ServiceResult<bool>> DeleteAsync(Guid applicationId, Guid id, CancellationToken ct);
}