namespace API.Binding;

public class ValidatedModelBinderBase<T>
{
    protected static Validated<T> CreateInvalid(string? propertyName, string errorMessage)
        => Validated.CreateInvalid<T>(
            new ValidationResult(
                false,
                new[] { new ValidationError(propertyName, errorMessage) }));
}
