namespace Common.Results;

public static class Result
{
    public static Result<T> Success<T>(T value) => new(value, null);
}

public record Result<T>(T? Value, ResultError? Error)
{
}

public record ResultError(
    bool IsValidationError,
    IEnumerable<ResultFailure>? ResultErrors,
    IEnumerable<ValidationFailure>? ValidationErrors);
