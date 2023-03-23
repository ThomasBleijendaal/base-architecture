namespace Common.Results;

public static class Result
{
    public static Result<T> Success<T>(T value)
        => new(value, ExecutionResult.Success);

    public static Result<T> ValidationError<T>(params ValidationFailure[] validationErrors)
        => new(default, new ExecutionResult(false, false, true, null, validationErrors));

    public static Result<T> TransientError<T>(params ResultFailure[] resultErrors)
        => new(default, new ExecutionResult(false, true, false, resultErrors, null));

    public static Result<T> ExecutionError<T>(params ResultFailure[] resultErrors)
        => new(default, new ExecutionResult(false, false, false, resultErrors, null));
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

    public IEnumerable<ResultFailure> ResultErrors => _execution.ResultErrors ?? Enumerable.Empty<ResultFailure>();

    public IEnumerable<ValidationFailure> ValidationErrors => _execution.ValidationErrors ?? Enumerable.Empty<ValidationFailure>();
}
