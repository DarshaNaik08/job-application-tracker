using static System.Net.Mime.MediaTypeNames;

namespace JobTracker.Api.Models
{
    public class JobApplication
    {

        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? WorkMode { get; set; }
        public string Source { get; set; } = string.Empty;
        public string? JobUrl { get; set; }
        public Boolean Status { get; set; }
        public string? AppliedOn { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
   
}
