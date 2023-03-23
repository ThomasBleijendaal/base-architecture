namespace Common.Results;

public record ExecutionResult(
    bool IsValid,
    bool IsTransientError,
    bool IsValidationError,
    IEnumerable<ResultFailure>? ResultErrors,
    IEnumerable<ValidationFailure>? ValidationErrors)
{
    public static readonly ExecutionResult Success = new(true, false, false, null, null);
}
