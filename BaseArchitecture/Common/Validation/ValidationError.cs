namespace Common.Validation;

public record ValidationError(string? PropertyName, string ErrorMessage);
