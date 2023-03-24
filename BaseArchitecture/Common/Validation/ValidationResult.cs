namespace Common.Validation;

public record ValidationResult(
    bool IsValid,
    IReadOnlyList<ValidationError>? ValidationErrors)
{
    public static readonly ValidationResult Success = new(true, null);

    internal static ValidationResult Map(FluentValidation.Results.ValidationResult result)
        => new(
            result.IsValid,
            result.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)).ToArray());
};
