namespace Common.Validation;

public class Validated
{
    public static async Task<Validated<T>> CreateAsync<T>(IValidator<T> validator, T model)
    {
        var results = await validator.ValidateAsync(model);
        return new Validated<T>(model, results);
    }

    public static Task<Validated<T>> CreateAsync<T>(IServiceProvider serviceProvider, T model)
    {
        var validator = serviceProvider.GetRequiredService<IValidator<T>>();
        return CreateAsync(validator, model);
    }

    public static Validated<T> CreateInvalid<T>(ValidationResult error)
    {
        return new Validated<T>(default, error);
    }
}

public class Validated<T> : IValidated
{
    private readonly T? _value;
    private readonly ValidationResult _validation;

    internal Validated(T? value, ValidationResult validation)
    {
        _value = value;
        _validation = validation;
    }

    /// <summary>
    /// Returns the validated value, or an exception if not valid.
    /// </summary>
    /// <exception cref="ValidationException"></exception>
    public T Value
        => (IsValid ? _value : default) ?? throw new ValidationException(_validation.Errors);

    public bool IsValid => _validation.IsValid;

    public IEnumerable<ValidationFailure> Errors => _validation.Errors;
}
