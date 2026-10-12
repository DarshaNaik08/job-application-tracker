using JobTracker.Api.Common;
using JobTracker.Api.Dtos;
using JobTracker.Api.Interfaces.Repositories;
using JobTracker.Api.Interfaces.Services;
using JobTracker.Api.Models;

namespace JobTracker.Api.Services;

public class CompanyService(ICompanyRepository repo) : ICompanyService
{
    public async Task<List<CompanyResponse>> GetAllAsync(CancellationToken ct)
    {
        var companies = await repo.GetAllAsync(ct);
        return companies.Select(ToResponse).ToList();
    }

    public async Task<ServiceResult<CompanyResponse>> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var company = await repo.GetByIdAsync(id, ct);
        return company is null
            ? ServiceResult<CompanyResponse>.NotFound()
            : ServiceResult<CompanyResponse>.Success(ToResponse(company));
    }

    public async Task<ServiceResult<CompanyResponse>> CreateAsync(CreateCompanyRequest req, CancellationToken ct)
    {
        var name = req.Name.Trim();

        if (await repo.NameExistsAsync(name, null, ct))
            return ServiceResult<CompanyResponse>.Conflict($"A company named '{name}' already exists.");

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = name,
            Website = req.Website?.Trim(),
            Location = req.Location?.Trim()
        };

        repo.Add(company);
        await repo.SaveChangesAsync(ct);

        return ServiceResult<CompanyResponse>.Success(ToResponse(company));
    }

    public async Task<ServiceResult<CompanyResponse>> UpdateAsync(Guid id, UpdateCompanyRequest req, CancellationToken ct)
    {
        var company = await repo.GetByIdAsync(id, ct);
        if (company is null) return ServiceResult<CompanyResponse>.NotFound();

        var name = req.Name.Trim();
        if (await repo.NameExistsAsync(name, id, ct))
            return ServiceResult<CompanyResponse>.Conflict($"A company named '{name}' already exists.");

        company.Name = name;
        company.Website = req.Website?.Trim();
        company.Location = req.Location?.Trim();

        await repo.SaveChangesAsync(ct);
        return ServiceResult<CompanyResponse>.Success(ToResponse(company));
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid id, CancellationToken ct)
    {
        var company = await repo.GetByIdAsync(id, ct);
        if (company is null) return ServiceResult<bool>.NotFound();

        if (await repo.HasApplicationsAsync(id, ct))
            return ServiceResult<bool>.Conflict("This company still has job applications. Delete those first.");

        repo.Remove(company);
        await repo.SaveChangesAsync(ct);
        return ServiceResult<bool>.Success(true);   // true only means "no payload"
    }

    private static CompanyResponse ToResponse(Company c) =>
        new(c.Id, c.Name, c.Website, c.Location);
}