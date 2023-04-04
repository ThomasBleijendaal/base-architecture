using Common;

namespace API.Binding;

public class ValidatedRouteModelBinder<T> : ValidatedModelBinderBase<T>, IModelBinder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        AllowTrailingCommas = true,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("BindRoute");

        var result = await ParseAndValidateModelAsync(bindingContext);

        activity?.AddTag("valid", result.IsValid);

        bindingContext.Result = ModelBindingResult.Success(result);
    }

    private static async Task<Validated<T>> ParseAndValidateModelAsync(ModelBindingContext bindingContext)
    {
        try
        {
            var value = bindingContext.HttpContext.Request.RouteValues.ToObject<T>(JsonOptions)
                ?? throw new ArgumentException(bindingContext.FieldName);

            return await Validated.CreateAsync(bindingContext.HttpContext.RequestServices, value);
        }
        catch (JsonException ex)
        {
            return CreateInvalid(ex.Path, "Invalid type");
        }
        catch
        {
            return CreateInvalid(bindingContext.FieldName, "Failed to parse model");
        }
    }
}
