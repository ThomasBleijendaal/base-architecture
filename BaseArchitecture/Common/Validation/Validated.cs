namespace Common.Validation;

public class Validated<T> : IValidated
{
    private ValidationResult Validation { get; }
    private readonly T? _value;

    private Validated(T? value, ValidationResult validation)
    {
        _value = value;
        Validation = validation;
    }

    /// <summary>
    /// Returns the validated value, or an exception if not valid.
    /// </summary>
    /// <exception cref="ValidationException"></exception>
    public T Value
        => (IsValid ? _value : default) ?? throw new ValidationException(Validation.Errors);

    public bool IsValid => Validation.IsValid;

    public IEnumerable<ValidationFailure> Errors => Validation.Errors;

    public void Deconstruct(out bool isValid, out T? value)
    {
        isValid = IsValid;
        value = Value;
    }

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
