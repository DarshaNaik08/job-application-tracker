using JobTracker.Api.Models;

namespace JobTracker.Api.Interfaces.Repositories;

public interface IInterviewRepository
{
    Task<List<Interview>> GetByApplicationAsync(Guid applicationId, CancellationToken ct);
    Task<Interview?> GetByIdAsync(Guid applicationId, Guid id, CancellationToken ct);
    void Add(Interview interview);
    void Remove(Interview interview);
    Task SaveChangesAsync(CancellationToken ct);
}