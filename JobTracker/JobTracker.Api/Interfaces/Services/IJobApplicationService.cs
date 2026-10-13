using JobTracker.Api.Common;
using JobTracker.Api.Dtos;

namespace JobTracker.Api.Interfaces.Services;

public interface IJobApplicationService
{
    Task<PagedResult<JobApplicationResponse>> SearchAsync(JobApplicationQuery query, CancellationToken ct);
    Task<ServiceResult<JobApplicationResponse>> GetByIdAsync(Guid id, CancellationToken ct);
    Task<ServiceResult<JobApplicationResponse>> CreateAsync(CreateJobApplicationRequest req, CancellationToken ct);
    Task<ServiceResult<JobApplicationResponse>> UpdateAsync(Guid id, UpdateJobApplicationRequest req, CancellationToken ct);
    Task<ServiceResult<JobApplicationResponse>> ChangeStatusAsync(Guid id, ChangeStatusRequest req, CancellationToken ct);
    Task<ServiceResult<bool>> DeleteAsync(Guid id, CancellationToken ct);
}