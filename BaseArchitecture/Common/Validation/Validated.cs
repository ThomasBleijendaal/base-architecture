namespace Common.Validation;

public class Validated<T> : IValidated
{
    private readonly T? _value;
    private readonly ValidationResult _validation;

    private Validated(T? value, ValidationResult validation)
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

    // TODO: move these to non-generic class?

    public static async Task<Validated<T>> CreateAsync(IValidator<T> validator, T model)
    {
        var results = await validator.ValidateAsync(model);
        return new Validated<T>(model, results);
    }

    public static Task<Validated<T>> CreateAsync(IServiceProvider serviceProvider, T model)
    {
        var validator = serviceProvider.GetRequiredService<IValidator<T>>();
        return CreateAsync(validator, model);
    }

    public static Validated<T> CreateInvalid(ValidationResult error)
    {
        return new Validated<T>(default, error);
    }
}
