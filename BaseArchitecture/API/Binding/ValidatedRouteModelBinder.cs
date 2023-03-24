namespace API.Binding;

// TODO: merge common stuff with the body binder
public class ValidatedRouteModelBinder<T> : IModelBinder
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
        var result = await ParseAndValidateModelAsync(bindingContext);
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
        catch (JsonException ex) when (ex.InnerException is InvalidOperationException castException)
        {
            return CreateInvalid(ex.Path, castException.Message);
        }
        catch (JsonException ex)
        {
            return CreateInvalid(ex.Path, ex.Message);
        }
        catch
        {
            return CreateInvalid(bindingContext.FieldName, "Failed to parse model");
        }
    }

    private static Validated<T> CreateInvalid(string? propertyName, string errorMessage)
        => Validated.CreateInvalid<T>(
            new ValidationResult(
                false,
                new[] { new ValidationError(propertyName, errorMessage) }));
}
