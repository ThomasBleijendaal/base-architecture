using Common.Validation;

namespace Common.Binding;

public class JsonModelBinder<T> : IModelBinder<T>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true
    };

    public async ValueTask<Validated<T>?> BindAndValidateAsync(HttpContext context, ParameterInfo parameter)
    {
        using var streamReader = new StreamReader(context.Request.Body);
        var json = await streamReader.ReadToEndAsync();

        try
        {
            var value = JsonSerializer.Deserialize<T>(json, JsonOptions)
                ?? throw new ArgumentException(parameter.Name);

            var validator = context.RequestServices.GetRequiredService<IValidator<T>>();

            var results = await validator.ValidateAsync(value);

            return new Validated<T>(value, results);
        }
        catch (JsonException ex) when (ex.InnerException is InvalidOperationException castException)
        {
            return new Validated<T>(default, new ValidationResult
            {
                Errors =
                {
                    new ValidationFailure(ex.Path, castException.Message)
                }
            });
        }
        catch (JsonException ex)
        {
            return new Validated<T>(default, new ValidationResult
            {
                Errors =
                {
                    new ValidationFailure(ex.Path, ex.Message)
                }
            });
        }
        catch (Exception)
        {
            return new Validated<T>(default, new ValidationResult
            {
                Errors =
                {
                    new ValidationFailure(parameter.Name, "Failed to parse model")
                }
            });
        }
    }
}
