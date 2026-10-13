using JobTracker.Api.Models;

namespace JobTracker.Api.Interfaces.Repositories;

public interface IJobApplicationRepository
{
    Task<(List<JobApplication> Items, int TotalCount)> SearchAsync(
        ApplicationStatus? status, Guid? companyId, string? search,
        string? sortBy, bool descending, int page, int pageSize, CancellationToken ct);

    Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken ct);
    void Add(JobApplication application);
    void Remove(JobApplication application);
    Task SaveChangesAsync(CancellationToken ct);
}