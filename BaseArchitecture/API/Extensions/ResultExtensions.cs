namespace API.Extensions;

public static class ResultExtensions
{
    public static ActionResult GetDefaultUnsuccessfulResult<T>(this Result<T> result)
        => result switch
        {
            // TODO: map validation errors to a response model
            { IsValidationError: true } => new BadRequestObjectResult(result.ValidationErrors),

            { IsTransientError: true } => CreateStatusCodeResult(result, StatusCodes.Status503ServiceUnavailable),

            _ => CreateStatusCodeResult(result, StatusCodes.Status500InternalServerError),
        };

    private static ObjectResult CreateStatusCodeResult<T>(Result<T> result, int statusCode)
        => new ObjectResult(result.ResultErrors)
        {
            StatusCode = statusCode
        };
}
