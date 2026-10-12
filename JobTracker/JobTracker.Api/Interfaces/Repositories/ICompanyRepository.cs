using JobTracker.Api.Models;

namespace JobTracker.Api.Interfaces.Repositories;

public interface ICompanyRepository
{
    Task<List<Company>> GetAllAsync(CancellationToken ct);
    Task<Company?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> NameExistsAsync(string name, Guid? excludeId, CancellationToken ct);
    Task<bool> HasApplicationsAsync(Guid companyId, CancellationToken ct);
    void Add(Company company);
    void Remove(Company company);
    Task SaveChangesAsync(CancellationToken ct);
}