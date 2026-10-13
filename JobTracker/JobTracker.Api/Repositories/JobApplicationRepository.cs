using JobTracker.Api.Data;
using JobTracker.Api.Interfaces.Repositories;
using JobTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api.Repositories;

public class JobApplicationRepository(AppDbContext db) : IJobApplicationRepository
{
    public async Task<(List<JobApplication> Items, int TotalCount)> SearchAsync(
        ApplicationStatus? status, Guid? companyId, string? search,
        string? sortBy, bool descending, int page, int pageSize, CancellationToken ct)
    {
        var query = db.JobApplications
            .AsNoTracking()
            .Include(a => a.Company)
            .AsQueryable();

        // 1. Filters: each one only applies when the caller supplied a value
        if (status is not null)
            query = query.Where(a => a.Status == status);

        if (companyId is not null)
            query = query.Where(a => a.CompanyId == companyId);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(a => a.Title.Contains(search) || a.Company.Name.Contains(search));

        // 2. Count ALL matching rows, before paging
        var total = await query.CountAsync(ct);

        // 3. Sorting
        IOrderedQueryable<JobApplication> ordered = sortBy?.ToLowerInvariant() switch
        {
            "title" => descending
                ? query.OrderByDescending(a => a.Title)
                : query.OrderBy(a => a.Title),
            "company" => descending
                ? query.OrderByDescending(a => a.Company.Name)
                : query.OrderBy(a => a.Company.Name),
            "status" => descending
                ? query.OrderByDescending(a => a.Status)
                : query.OrderBy(a => a.Status),
            "appliedon" => descending
                ? query.OrderByDescending(a => a.AppliedOn)
                : query.OrderBy(a => a.AppliedOn),
            _ => descending
                ? query.OrderByDescending(a => a.CreatedAt)
                : query.OrderBy(a => a.CreatedAt)
        };

        // 4. Paging, plus a tie-breaker so rows with equal values keep a stable order
        var items = await ordered
            .ThenBy(a => a.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken ct) =>
        db.JobApplications.Include(a => a.Company).FirstOrDefaultAsync(a => a.Id == id, ct);

    public void Add(JobApplication application) => db.JobApplications.Add(application);

    public void Remove(JobApplication application) => db.JobApplications.Remove(application);

    public Task SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct) => db.JobApplications.AnyAsync(a => a.Id == id, ct);
}