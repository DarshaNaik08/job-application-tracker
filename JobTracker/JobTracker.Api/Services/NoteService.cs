using JobTracker.Api.Common;
using JobTracker.Api.Dtos;
using JobTracker.Api.Interfaces.Repositories;
using JobTracker.Api.Interfaces.Services;
using JobTracker.Api.Models;

namespace JobTracker.Api.Services;

public class NoteService(
    INoteRepository notes,
    IJobApplicationRepository applications) : INoteService
{
    public async Task<ServiceResult<List<NoteResponse>>> GetAllAsync(Guid applicationId, CancellationToken ct)
    {
        if (!await applications.ExistsAsync(applicationId, ct))
            return ServiceResult<List<NoteResponse>>.NotFound("Job application not found.");

        var items = await notes.GetByApplicationAsync(applicationId, ct);
        return ServiceResult<List<NoteResponse>>.Success(items.Select(ToResponse).ToList());
    }

    public async Task<ServiceResult<NoteResponse>> CreateAsync(
        Guid applicationId, CreateNoteRequest req, CancellationToken ct)
    {
        if (!await applications.ExistsAsync(applicationId, ct))
            return ServiceResult<NoteResponse>.NotFound("Job application not found.");

        var note = new ApplicationNote
        {
            Id = Guid.NewGuid(),
            JobApplicationId = applicationId,
            Text = req.Text.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        notes.Add(note);
        await notes.SaveChangesAsync(ct);

        return ServiceResult<NoteResponse>.Success(ToResponse(note));
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid applicationId, Guid id, CancellationToken ct)
    {
        var note = await notes.GetByIdAsync(applicationId, id, ct);
        if (note is null) return ServiceResult<bool>.NotFound();

        notes.Remove(note);
        await notes.SaveChangesAsync(ct);
        return ServiceResult<bool>.Success(true);
    }

    private static NoteResponse ToResponse(ApplicationNote n) =>
        new(n.Id, n.JobApplicationId, n.Text, n.CreatedAt);
}