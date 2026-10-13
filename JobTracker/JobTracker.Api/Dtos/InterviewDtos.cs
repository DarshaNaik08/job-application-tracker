using System.ComponentModel.DataAnnotations;

namespace JobTracker.Api.Dtos;

public record CreateInterviewRequest(
    [Required, StringLength(100)] string Type,
    [Required] DateTime? ScheduledAt,
    [StringLength(500)] string? Outcome);

public record UpdateInterviewRequest(
    [Required, StringLength(100)] string Type,
    [Required] DateTime? ScheduledAt,
    [StringLength(500)] string? Outcome);

public record InterviewResponse(
    Guid Id,
    Guid JobApplicationId,
    string Type,
    DateTime ScheduledAt,
    string? Outcome);