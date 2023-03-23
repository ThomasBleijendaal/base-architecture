namespace API.Binding;

// TODO: merge common stuff with the body binder
public class ValidatedQueryModelBinder<T> : IModelBinder
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
            var value = bindingContext.HttpContext.Request.Query.ToObject<T>(JsonOptions)
                ?? throw new ArgumentException(bindingContext.FieldName);

            return await Validated.CreateAsync<T>(bindingContext.HttpContext.RequestServices, value);
        }
        catch (JsonException ex) when (ex.InnerException is InvalidOperationException castException)
        {
            return Validated.CreateInvalid<T>(new ValidationResult
            {
                Errors =
                {
                    new ValidationFailure(ex.Path, castException.Message)
                }
            });
        }
        catch (JsonException ex)
        {
            return Validated.CreateInvalid<T>(new ValidationResult
            {
                Errors =
                {
                    new ValidationFailure(ex.Path, ex.Message)
                }
            });
        }
        catch
        {
            return Validated.CreateInvalid<T>(new ValidationResult
            {
                Errors =
                {
                    new ValidationFailure(bindingContext.FieldName, "Failed to parse model")
                }
            });
        }
    }
}
