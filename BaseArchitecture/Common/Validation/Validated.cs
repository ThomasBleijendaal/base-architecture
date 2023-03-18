using Common.Binding;

namespace Common.Validation;

// TODO: add support to statically set value so it can be validated
public class Validated<T>
{
    private ValidationResult Validation { get; }
    private readonly T? _value;

    public Validated()
    {

    }

    internal Validated(T? value, ValidationResult validation)
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

    public IDictionary<string, string[]> Errors =>
        Validation
            .Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

    public void Deconstruct(out bool isValid, out T? value)
    {
        isValid = IsValid;
        value = Value;
    }

    public static ValueTask<Validated<T>?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var genericBinderType = parameter.GetCustomAttribute<BinderAttribute>()?.ModelBinderType ?? typeof(JsonModelBinder<>);
        var binderType = genericBinderType.MakeGenericType(typeof(T));

        if (context.RequestServices.GetRequiredService(binderType) is not IModelBinder<T> binder)
        {
            return new ValueTask<Validated<T>?>(new Validated<T>(default, new ValidationResult
            {
                Errors =
                {
                    new ValidationFailure(parameter.Name, "Failed to get correct model binder")
                }
            }));
        }

        return binder.BindAndValidateAsync(context, parameter);
    }
}
