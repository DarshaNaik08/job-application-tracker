using JobTracker.Api.Models;

namespace JobTracker.Api.Interfaces.Repositories;

public interface INoteRepository
{
    Task<List<ApplicationNote>> GetByApplicationAsync(Guid applicationId, CancellationToken ct);
    Task<ApplicationNote?> GetByIdAsync(Guid applicationId, Guid id, CancellationToken ct);
    void Add(ApplicationNote note);
    void Remove(ApplicationNote note);
    Task SaveChangesAsync(CancellationToken ct);
}