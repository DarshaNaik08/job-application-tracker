namespace JobTracker.Api.Common;

public enum ResultStatus { Success, NotFound, Conflict }

public record ServiceResult<T>(ResultStatus Status, T? Value = default, string? Message = null)
{
    public bool IsSuccess => Status == ResultStatus.Success;

    public static ServiceResult<T> Success(T value) => new(ResultStatus.Success, value);
    public static ServiceResult<T> NotFound(string? message = null) => new(ResultStatus.NotFound, default, message);
    public static ServiceResult<T> Conflict(string message) => new(ResultStatus.Conflict, default, message);
}