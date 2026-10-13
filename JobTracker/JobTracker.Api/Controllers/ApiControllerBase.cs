using JobTracker.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult<T> ToActionResult<T>(ServiceResult<T> result)
    {
        if (!result.IsSuccess) return ToError(result);
        return Ok(result.Value);
    }

    protected ActionResult ToError<T>(ServiceResult<T> result) => result.Status switch
    {
        ResultStatus.NotFound => NotFound(result.Message),
        ResultStatus.Conflict => Conflict(result.Message),
        ResultStatus.Invalid => BadRequest(result.Message),
        _ => StatusCode(StatusCodes.Status500InternalServerError)
    };
}