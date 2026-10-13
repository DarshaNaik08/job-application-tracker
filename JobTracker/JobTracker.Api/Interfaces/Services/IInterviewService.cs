using JobTracker.Api.Common;
using JobTracker.Api.Dtos;

namespace JobTracker.Api.Interfaces.Services;

public interface IInterviewService
{
    Task<ServiceResult<List<InterviewResponse>>> GetAllAsync(Guid applicationId, CancellationToken ct);
    Task<ServiceResult<InterviewResponse>> GetByIdAsync(Guid applicationId, Guid id, CancellationToken ct);
    Task<ServiceResult<InterviewResponse>> CreateAsync(Guid applicationId, CreateInterviewRequest req, CancellationToken ct);
    Task<ServiceResult<InterviewResponse>> UpdateAsync(Guid applicationId, Guid id, UpdateInterviewRequest req, CancellationToken ct);
    Task<ServiceResult<bool>> DeleteAsync(Guid applicationId, Guid id, CancellationToken ct);
}