using JobTracker.Api.Common;
using JobTracker.Api.Dtos;
using JobTracker.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController(ICompanyService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CompanyResponse>>> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CompanyResponse>> GetById(Guid id, CancellationToken ct) =>
        ToActionResult(await service.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<CompanyResponse>> Create(CreateCompanyRequest req, CancellationToken ct)
    {
        var result = await service.CreateAsync(req, ct);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);

        return ToActionResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CompanyResponse>> Update(
        Guid id, UpdateCompanyRequest req, CancellationToken ct) =>
        ToActionResult(await service.UpdateAsync(id, req, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await service.DeleteAsync(id, ct);

        return result.Status switch
        {
            ResultStatus.Success => NoContent(),
            ResultStatus.NotFound => NotFound(result.Message),
            ResultStatus.Conflict => Conflict(result.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    // Translates a service outcome into an HTTP response
    private ActionResult<T> ToActionResult<T>(ServiceResult<T> result)
    {
        if (result.Status == ResultStatus.NotFound) return NotFound(result.Message);
        if (result.Status == ResultStatus.Conflict) return Conflict(result.Message);
        return Ok(result.Value);
    }
}