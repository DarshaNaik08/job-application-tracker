using System.ComponentModel.DataAnnotations;

namespace JobTracker.Api.Dtos;

public record CreateCompanyRequest(
    [Required, StringLength(200)] string Name,
    [StringLength(500)] string? Website,
    [StringLength(200)] string? Location);

public record UpdateCompanyRequest(
    [Required, StringLength(200)] string Name,
    [StringLength(500)] string? Website,
    [StringLength(200)] string? Location);

public record CompanyResponse(Guid Id, string Name, string? Website, string? Location);