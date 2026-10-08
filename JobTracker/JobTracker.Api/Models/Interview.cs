namespace JobTracker.Api.Models;

public class Interview
{
    public Guid Id { get; set; }

    public Guid JobApplicationId { get; set; }
    public JobApplication JobApplication { get; set; } = null!;
    public string Type { get; set; } = string.Empty;   // e.g. "Phone screen", "Technical", "HR"
    public DateTime ScheduledAt { get; set; }
    public string? Outcome { get; set; }               // filled in after the interview
}