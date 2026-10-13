using System.ComponentModel.DataAnnotations;
using JobTracker.Api.Models;

namespace JobTracker.Api.Dtos;

public record CreateJobApplicationRequest(
    [Required] Guid CompanyId,
    [Required, StringLength(200)] string Title,
    [StringLength(200)] string? Location,
    WorkMode? WorkMode,
    [Required, StringLength(100)] string Source,
    [Url, StringLength(1000)] string? JobUrl,
    ApplicationStatus Status,
    DateOnly? AppliedOn);

public record UpdateJobApplicationRequest(
    [Required] Guid CompanyId,
    [Required, StringLength(200)] string Title,
    [StringLength(200)] string? Location,
    WorkMode? WorkMode,
    [Required, StringLength(100)] string Source,
    [Url, StringLength(1000)] string? JobUrl,
    DateOnly? AppliedOn);

public record ChangeStatusRequest(ApplicationStatus Status);

public record JobApplicationResponse(
    Guid Id,
    Guid CompanyId,
    string CompanyName,
    string Title,
    string? Location,
    WorkMode? WorkMode,
    string Source,
    string? JobUrl,
    ApplicationStatus Status,
    DateOnly? AppliedOn,
    DateTime CreatedAt,
    DateTime UpdatedAt);

// Bound from the query string: GET /api/job-applications?status=Applied&page=2
public class JobApplicationQuery
{
    public ApplicationStatus? Status { get; set; }
    public Guid? CompanyId { get; set; }
    public string? Search { get; set; }
    public string? SortBy { get; set; }      // appliedOn | title | company | status | createdAt
    public bool Desc { get; set; } = true;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public record PagedResult<T>(List<T> Items, int Page, int PageSize, int TotalCount)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}