using JobTracker.Api.Common;
using JobTracker.Api.Dtos;

namespace JobTracker.Api.Interfaces.Services;

public interface ICompanyService
{
    Task<List<CompanyResponse>> GetAllAsync(CancellationToken ct);
    Task<ServiceResult<CompanyResponse>> GetByIdAsync(Guid id, CancellationToken ct);
    Task<ServiceResult<CompanyResponse>> CreateAsync(CreateCompanyRequest req, CancellationToken ct);
    Task<ServiceResult<CompanyResponse>> UpdateAsync(Guid id, UpdateCompanyRequest req, CancellationToken ct);
    Task<ServiceResult<bool>> DeleteAsync(Guid id, CancellationToken ct);
}