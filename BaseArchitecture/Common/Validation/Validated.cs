namespace Common.Validation;

public class Validated
{
    public static async Task<Validated<T>> CreateAsync<T>(FluentValidation.IValidator<T> validator, T model)
    {
        var validationResults = await validator.ValidateAsync(model);

        return new Validated<T>(model, ValidationResult.Map(validationResults));
    }

    public static Task<Validated<T>> CreateAsync<T>(IServiceProvider serviceProvider, T model)
    {
        var validator = serviceProvider.GetRequiredService<FluentValidation.IValidator<T>>();
        return CreateAsync(validator, model);
    }

    public static Validated<T> CreateInvalid<T>(ValidationResult error)
        => new(default, error);
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
        => (IsValid ? _value : default) ?? throw new ValidationException(Errors);

    public bool IsValid => _validation.IsValid;

    public IEnumerable<ValidationError> Errors
        => _validation.ValidationErrors ?? Enumerable.Empty<ValidationError>();
}
