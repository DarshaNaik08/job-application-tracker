namespace JobTracker.Api.Models;

public class ApplicationNote
{
    public Guid Id { get; set; }
    public Guid JobApplicationId { get; set; }
    public JobApplication JobApplication { get; set; } = null!;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}