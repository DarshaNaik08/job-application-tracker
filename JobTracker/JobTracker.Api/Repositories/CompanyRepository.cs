using JobTracker.Api.Data;
using JobTracker.Api.Interfaces.Repositories;
using JobTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api.Repositories;

public class CompanyRepository(AppDbContext db) : ICompanyRepository
{
    public Task<List<Company>> GetAllAsync(CancellationToken ct) =>
        db.Companies.AsNoTracking().OrderBy(c => c.Name).ToListAsync(ct);

    public async Task<Company?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await db.Companies.FindAsync([id], ct);

    public Task<bool> NameExistsAsync(string name, Guid? excludeId, CancellationToken ct) =>
        db.Companies.AnyAsync(c => c.Name == name && (excludeId == null || c.Id != excludeId), ct);

    public Task<bool> HasApplicationsAsync(Guid companyId, CancellationToken ct) =>
        db.JobApplications.AnyAsync(a => a.CompanyId == companyId, ct);

    public void Add(Company company) => db.Companies.Add(company);

    public void Remove(Company company) => db.Companies.Remove(company);

    public Task SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}