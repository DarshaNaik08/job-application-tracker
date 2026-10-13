using JobTracker.Api.Common;
using JobTracker.Api.Dtos;
using JobTracker.Api.Interfaces.Repositories;
using JobTracker.Api.Interfaces.Services;
using JobTracker.Api.Models;

namespace JobTracker.Api.Services;

public class JobApplicationService(
    IJobApplicationRepository applications,
    ICompanyRepository companies) : IJobApplicationService
{
    public async Task<PagedResult<JobApplicationResponse>> SearchAsync(JobApplicationQuery q, CancellationToken ct)
    {
        var page = Math.Max(q.Page, 1);
        var pageSize = Math.Clamp(q.PageSize, 1, 100);

        var (items, total) = await applications.SearchAsync(
            q.Status, q.CompanyId, q.Search?.Trim(), q.SortBy, q.Desc, page, pageSize, ct);

        return new PagedResult<JobApplicationResponse>(
            items.Select(ToResponse).ToList(), page, pageSize, total);
    }

    public async Task<ServiceResult<JobApplicationResponse>> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var application = await applications.GetByIdAsync(id, ct);
        return application is null
            ? ServiceResult<JobApplicationResponse>.NotFound()
            : ServiceResult<JobApplicationResponse>.Success(ToResponse(application));
    }

    public async Task<ServiceResult<JobApplicationResponse>> CreateAsync(CreateJobApplicationRequest req, CancellationToken ct)
    {
        var company = await companies.GetByIdAsync(req.CompanyId, ct);
        if (company is null)
            return ServiceResult<JobApplicationResponse>.Invalid("The selected company does not exist.");

        var now = DateTime.UtcNow;
        var application = new JobApplication
        {
            Id = Guid.NewGuid(),
            CompanyId = company.Id,
            Company = company,
            Title = req.Title.Trim(),
            Location = req.Location?.Trim(),
            WorkMode = req.WorkMode,
            Source = req.Source.Trim(),
            JobUrl = req.JobUrl?.Trim(),
            Status = req.Status,
            AppliedOn = req.AppliedOn,
            CreatedAt = now,
            UpdatedAt = now
        };

        SetAppliedOnIfNeeded(application);

        applications.Add(application);
        await applications.SaveChangesAsync(ct);

        return ServiceResult<JobApplicationResponse>.Success(ToResponse(application));
    }

    public async Task<ServiceResult<JobApplicationResponse>> UpdateAsync(
        Guid id, UpdateJobApplicationRequest req, CancellationToken ct)
    {
        var application = await applications.GetByIdAsync(id, ct);
        if (application is null) return ServiceResult<JobApplicationResponse>.NotFound();

        if (application.CompanyId != req.CompanyId)
        {
            var company = await companies.GetByIdAsync(req.CompanyId, ct);
            if (company is null)
                return ServiceResult<JobApplicationResponse>.Invalid("The selected company does not exist.");

            application.CompanyId = company.Id;
            application.Company = company;
        }

        application.Title = req.Title.Trim();
        application.Location = req.Location?.Trim();
        application.WorkMode = req.WorkMode;
        application.Source = req.Source.Trim();
        application.JobUrl = req.JobUrl?.Trim();
        application.AppliedOn = req.AppliedOn;
        application.UpdatedAt = DateTime.UtcNow;

        await applications.SaveChangesAsync(ct);
        return ServiceResult<JobApplicationResponse>.Success(ToResponse(application));
    }

    public async Task<ServiceResult<JobApplicationResponse>> ChangeStatusAsync(
        Guid id, ChangeStatusRequest req, CancellationToken ct)
    {
        var application = await applications.GetByIdAsync(id, ct);
        if (application is null) return ServiceResult<JobApplicationResponse>.NotFound();

        application.Status = req.Status;
        SetAppliedOnIfNeeded(application);
        application.UpdatedAt = DateTime.UtcNow;

        await applications.SaveChangesAsync(ct);
        return ServiceResult<JobApplicationResponse>.Success(ToResponse(application));
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid id, CancellationToken ct)
    {
        var application = await applications.GetByIdAsync(id, ct);
        if (application is null) return ServiceResult<bool>.NotFound();

        applications.Remove(application);      // interviews and notes cascade
        await applications.SaveChangesAsync(ct);
        return ServiceResult<bool>.Success(true);
    }

    // Business rule: moving to "Applied" with no date records today's date
    private static void SetAppliedOnIfNeeded(JobApplication a)
    {
        if (a.Status == ApplicationStatus.Applied && a.AppliedOn is null)
            a.AppliedOn = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    private static JobApplicationResponse ToResponse(JobApplication a) => new(
        a.Id, a.CompanyId, a.Company.Name, a.Title, a.Location, a.WorkMode,
        a.Source, a.JobUrl, a.Status, a.AppliedOn, a.CreatedAt, a.UpdatedAt);
}