namespace API.Filters;

public class ValidatedContentFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionDescriptor is ControllerActionDescriptor controllerAction &&
            controllerAction.MethodInfo.GetCustomAttribute<AllowInvalidModelAttribute>() is not null)
        {
            return;
        }

        if (context.ActionArguments.Values.OfType<IValidated>().Any(x => !x.IsValid))
        {
            context.Result = BadRequest(context.ActionArguments.Values.OfType<IValidated>().SelectMany(x => x.Errors));
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            context.Exception = null;
            context.Result = BadRequest(validationException.Errors);
        }
    }

    private static ObjectResult BadRequest(IEnumerable<ValidationFailure> failures)
    {
        var content = failures
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                x => x.Key,
                x => x.Select(x => x.ErrorMessage));

        return new ObjectResult(content)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}
