using Common;

namespace API.Filters;

public class ValidatedContentFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("ValidatedActionExecuting");

        if (context.ActionDescriptor is ControllerActionDescriptor controllerAction &&
            controllerAction.MethodInfo.GetCustomAttribute<AllowInvalidModelAttribute>() is not null)
        {
            return;
        }

        var validatedArguments = context.ActionArguments.Values
            .OfType<IValidated>()
            .Where(x => !x.IsValid)
            .ToArray();

        if (validatedArguments is { Length: > 0 } invalidArguments)
        {
            context.Result = invalidArguments.GetDefaultResult();
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            using var activity = DiagnosticsConfig.ActivitySource.StartActivity("ValidationExceptionHandling");

            context.Exception = null;
            context.Result = validationException.ValidationErrors.GetDefaultResult();
        }
    }
}
