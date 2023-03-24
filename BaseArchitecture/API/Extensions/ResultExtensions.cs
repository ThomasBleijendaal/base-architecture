namespace API.Extensions;

public static class ResultExtensions
{
    public static ActionResult GetDefaultResult(this IEnumerable<ValidationError> errors)
        => new BadRequestObjectResult(ValidationErrorResponseModel.Map(errors));

    public static ActionResult GetDefaultResult(this IValidated validated)
        => new BadRequestObjectResult(ValidationErrorResponseModel.Map(validated.Errors));

    public static ActionResult GetDefaultResult(this IValidated[] validated)
        => new BadRequestObjectResult(ValidationErrorResponseModel.Map(validated.SelectMany(x => x.Errors)));

    public static ActionResult GetDefaultResult<T>(this Result<T> result)
        => result switch
        {
            { IsValidationError: true } => new BadRequestObjectResult(ValidationErrorResponseModel.Map(result.ValidationErrors)),

            { IsTransientError: true } => CreateStatusCodeResult(ResultErrorResponseModel.Map(result.ResultErrors), StatusCodes.Status503ServiceUnavailable),

            { IsSuccess: true } => new NoContentResult(),

            _ => CreateStatusCodeResult(ResultErrorResponseModel.Map(result.ResultErrors), StatusCodes.Status500InternalServerError),
        };

    private static ObjectResult CreateStatusCodeResult<T>(T result, int statusCode)
        => new(result)
        {
            StatusCode = statusCode
        };
}
