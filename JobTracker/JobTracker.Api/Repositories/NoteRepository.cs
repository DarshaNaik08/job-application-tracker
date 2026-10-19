using JobTracker.Api.Data;
using JobTracker.Api.Interfaces.Repositories;
using JobTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api.Repositories;

public class NoteRepository(AppDbContext db) : INoteRepository
{
    public Task<List<ApplicationNote>> GetByApplicationAsync(Guid applicationId, CancellationToken ct) =>
        db.Notes
            .AsNoTracking()
            .Where(n => n.JobApplicationId == applicationId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(ct);

    // Both ids must match, so a note can't be reached through another application's URL
    public Task<ApplicationNote?> GetByIdAsync(Guid applicationId, Guid id, CancellationToken ct) =>
        db.Notes.FirstOrDefaultAsync(n => n.Id == id && n.JobApplicationId == applicationId, ct);

    public void Add(ApplicationNote note) => db.Notes.Add(note);

    public void Remove(ApplicationNote note) => db.Notes.Remove(note);

    public Task SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}