using System.ComponentModel.DataAnnotations;

namespace JobTracker.Api.Dtos;

public record CreateNoteRequest(
    [Required, StringLength(2000)] string Text);

public record NoteResponse(
    Guid Id,
    Guid JobApplicationId,
    string Text,
    DateTime CreatedAt);