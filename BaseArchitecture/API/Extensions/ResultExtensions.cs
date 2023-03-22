using Common.Results;

namespace API.Extensions;

public static class ResultExtensions
{
    public static ActionResult GetUnsuccessfulResult<T>(this Result<T> result)
    {
        // TODO: add response content

        return result switch
        {
            { IsValidationError: true } => new BadRequestResult(),

            { IsTransientError: true } => new StatusCodeResult(StatusCodes.Status503ServiceUnavailable),

            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError),
        };
    }
}
