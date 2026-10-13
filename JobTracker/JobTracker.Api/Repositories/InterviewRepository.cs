using JobTracker.Api.Data;
using JobTracker.Api.Interfaces.Repositories;
using JobTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api.Repositories;

public class InterviewRepository(AppDbContext db) : IInterviewRepository
{
    public Task<List<Interview>> GetByApplicationAsync(Guid applicationId, CancellationToken ct) =>
        db.Interviews
            .AsNoTracking()
            .Where(i => i.JobApplicationId == applicationId)
            .OrderBy(i => i.ScheduledAt)
            .ToListAsync(ct);

    // The id AND the application id must both match, so an interview can't be
    // reached through a different application's URL
    public Task<Interview?> GetByIdAsync(Guid applicationId, Guid id, CancellationToken ct) =>
        db.Interviews.FirstOrDefaultAsync(
            i => i.Id == id && i.JobApplicationId == applicationId, ct);

    public void Add(Interview interview) => db.Interviews.Add(interview);

    public void Remove(Interview interview) => db.Interviews.Remove(interview);

    public Task SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}