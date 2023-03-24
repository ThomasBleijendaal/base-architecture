namespace Common.Results;

public record ExecutionResult(
    bool IsValid,
    bool IsTransientError,
    bool IsValidationError,
    IReadOnlyList<ResultError>? ResultErrors,
    IReadOnlyList<ValidationError>? ValidationErrors)
{
    public static readonly ExecutionResult Success = new(true, false, false, null, null);
}
