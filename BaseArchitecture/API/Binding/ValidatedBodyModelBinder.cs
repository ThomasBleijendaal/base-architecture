using Common;

namespace API.Binding;

public class ValidatedBodyModelBinder<T> : ValidatedModelBinderBase<T>, IModelBinder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true
    };

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("BindBody");

        var result = await ParseAndValidateModelAsync(bindingContext);

        activity?.AddTag("valid", result.IsValid);

        bindingContext.Result = ModelBindingResult.Success(result);
    }

    private static async Task<Validated<T>> ParseAndValidateModelAsync(ModelBindingContext bindingContext)
    {
        using var streamReader = new StreamReader(bindingContext.HttpContext.Request.Body);
        var json = await streamReader.ReadToEndAsync();

        try
        {
            var value = JsonSerializer.Deserialize<T>(json, JsonOptions)
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
