namespace JobTracker.Api.Models;

public class JobApplication
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string? Location { get; set; }
    public WorkMode? WorkMode { get; set; }
    public string Source { get; set; } = string.Empty;
    public string? JobUrl { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Wishlist;
    public DateOnly? AppliedOn { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<Interview> Interviews { get; set; } = [];
    public List<ApplicationNote> Notes { get; set; } = [];
}