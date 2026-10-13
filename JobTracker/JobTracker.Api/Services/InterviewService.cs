using JobTracker.Api.Common;
using JobTracker.Api.Dtos;
using JobTracker.Api.Interfaces.Repositories;
using JobTracker.Api.Interfaces.Services;
using JobTracker.Api.Models;

namespace JobTracker.Api.Services;

public class InterviewService(
    IInterviewRepository interviews,
    IJobApplicationRepository applications) : IInterviewService
{
    public async Task<ServiceResult<List<InterviewResponse>>> GetAllAsync(
        Guid applicationId, CancellationToken ct)
    {
        if (!await applications.ExistsAsync(applicationId, ct))
            return ServiceResult<List<InterviewResponse>>.NotFound("Job application not found.");

        var items = await interviews.GetByApplicationAsync(applicationId, ct);
        return ServiceResult<List<InterviewResponse>>.Success(items.Select(ToResponse).ToList());
    }

    public async Task<ServiceResult<InterviewResponse>> GetByIdAsync(
        Guid applicationId, Guid id, CancellationToken ct)
    {
        var interview = await interviews.GetByIdAsync(applicationId, id, ct);
        return interview is null
            ? ServiceResult<InterviewResponse>.NotFound()
            : ServiceResult<InterviewResponse>.Success(ToResponse(interview));
    }

    public async Task<ServiceResult<InterviewResponse>> CreateAsync(
     Guid applicationId, CreateInterviewRequest req, CancellationToken ct)
    {
        if (!await applications.ExistsAsync(applicationId, ct))
            return ServiceResult<InterviewResponse>.NotFound("Job application not found.");

        var interview = new Interview
        {
            Id = Guid.NewGuid(),
            JobApplicationId = applicationId,
            Type = req.Type.Trim(),
            ScheduledAt = req.ScheduledAt!.Value,
            Outcome = string.IsNullOrWhiteSpace(req.Outcome) ? null : req.Outcome.Trim()
        };

        interviews.Add(interview);
        await interviews.SaveChangesAsync(ct);

        return ServiceResult<InterviewResponse>.Success(ToResponse(interview));
    }

    public async Task<ServiceResult<InterviewResponse>> UpdateAsync(
        Guid applicationId, Guid id, UpdateInterviewRequest req, CancellationToken ct)
    {
        var interview = await interviews.GetByIdAsync(applicationId, id, ct);
        if (interview is null) return ServiceResult<InterviewResponse>.NotFound();

        interview.Type = req.Type.Trim();
        interview.ScheduledAt = req.ScheduledAt!.Value;
        interview.Outcome = string.IsNullOrWhiteSpace(req.Outcome) ? null : req.Outcome.Trim();

        await interviews.SaveChangesAsync(ct);
        return ServiceResult<InterviewResponse>.Success(ToResponse(interview));
    }
    public async Task<ServiceResult<bool>> DeleteAsync(Guid applicationId, Guid id, CancellationToken ct)
    {
        var interview = await interviews.GetByIdAsync(applicationId, id, ct);
        if (interview is null) return ServiceResult<bool>.NotFound();

        interviews.Remove(interview);
        await interviews.SaveChangesAsync(ct);
        return ServiceResult<bool>.Success(true);
    }

    private static InterviewResponse ToResponse(Interview i) =>
        new(i.Id, i.JobApplicationId, i.Type, i.ScheduledAt, i.Outcome);
}