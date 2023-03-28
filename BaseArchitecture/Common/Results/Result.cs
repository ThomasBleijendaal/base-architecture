namespace Common.Results;

public record Result
{
    protected readonly ExecutionResult _execution;

    public Result(ExecutionResult execution)
    {
        _execution = execution;
    }

    public bool IsSuccess => _execution.IsValid;

    public bool IsTransientError => _execution.IsTransientError;

    public bool IsValidationError => _execution.IsValidationError;

    public IReadOnlyList<ResultError> ResultErrors => _execution.ResultErrors ?? Array.Empty<ResultError>();

    public IReadOnlyList<ValidationError> ValidationErrors => _execution.ValidationErrors ?? Array.Empty<ValidationError>();

    public static Result Success()
        => new(ExecutionResult.Success);

    public static Result<T> Success<T>(T value)
        => new(value, ExecutionResult.Success);

    public static Result ValidationError(ValidationResult validationResult)
        => new(new ExecutionResult(false, false, !validationResult.IsValid, null, validationResult.ValidationErrors));

    public static Result<T> ValidationError<T>(ValidationResult validationResult)
        => new(default, new ExecutionResult(false, false, !validationResult.IsValid, null, validationResult.ValidationErrors));

    public static Result TransientError(params ResultError[] resultErrors)
        => new(new ExecutionResult(false, true, false, resultErrors, null));

    public static Result<T> TransientError<T>(params ResultError[] resultErrors)
        => new(default, new ExecutionResult(false, true, false, resultErrors, null));

    public static Result ExecutionError(params ResultError[] resultErrors)
        => new(new ExecutionResult(false, false, false, resultErrors, null));

    public static Result<T> ExecutionError<T>(params ResultError[] resultErrors)
        => new(default, new ExecutionResult(false, false, false, resultErrors, null));
}

public record Result<T> : Result
{
    private readonly T? _value;

    public Result(T? value, ExecutionResult execution) : base(execution)
    {
        _value = value;
    }

    public T? Value => _execution.IsValid ? _value
        : _execution.IsTransientError ? throw new TransientException(ResultErrors)
        : _execution.IsValidationError ? throw new ValidationException(ValidationErrors)
        : throw new ResultException(ResultErrors);

}
