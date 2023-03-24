namespace API.Models;

public record ValidationErrorResponseModel(Dictionary<string, IEnumerable<string>> Errors)
{
    public static ValidationErrorResponseModel Map(IEnumerable<ValidationError> errors)
        => new(errors
            .GroupBy(x => x.PropertyName ?? "")
            .ToDictionary(
                x => x.Key,
                x => x.Select(x => x.ErrorMessage)));
}

public record ResultErrorResponseModel(Dictionary<int, string> Errors)
{
    public static ResultErrorResponseModel Map(IEnumerable<ResultError> errors)
        => new(errors
            .ToDictionary(
                x => x.ErrorCode,
                x => x.ErrorMessage));
}
