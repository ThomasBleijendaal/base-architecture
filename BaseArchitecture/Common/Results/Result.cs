namespace Common.Results;

public static class Result
{
    public static Result<T> Success<T>(T value)
        => new(value, ExecutionResult.Success);

    public static Result<T> ValidationError<T>(IEnumerable<ValidationFailure> validationErrors)
        => new(default, new ExecutionResult(false, true, null, validationErrors));
}

public record Result<T>
{
    private readonly T? _value;
    private readonly ExecutionResult _execution;

    public Result(T? value, ExecutionResult execution)
    {
        _value = value;
        _execution = execution;
    }

    // TODO: what should transient error be
    // TODO: other errors?
    // TODO: should this be InternalValidationException?
    public T? Value => _execution.IsValid ? _value
        : _execution.IsTransientError ? throw new InvalidOperationException()
        : _execution.IsValidationError ? throw new ValidationException(_execution.ValidationErrors)
        : throw new InvalidOperationException();

    public bool IsSuccess => _execution.IsValid;

    public bool IsTransientError => _execution.IsTransientError;

    public bool IsValidationError => _execution.IsValidationError;
}

public record ExecutionResult(
    bool IsTransientError,
    bool IsValidationError,
    IEnumerable<ResultFailure>? ResultErrors,
    IEnumerable<ValidationFailure>? ValidationErrors)
{
    public static readonly ExecutionResult Success = new(false, false, null, null);

    public bool IsValid => !IsTransientError && !IsValidationError;
}
