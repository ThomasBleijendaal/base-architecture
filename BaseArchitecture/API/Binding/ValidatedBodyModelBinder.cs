using System.Text.Json;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Binding;

public class ValidatedBodyModelBinder<T> : IModelBinder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true
    };

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var result = await ParseAndValidateModelAsync(bindingContext);
        bindingContext.Result = ModelBindingResult.Success(result);
    }

    private static async Task<Validated<T>> ParseAndValidateModelAsync(ModelBindingContext bindingContext)
    {
        using var streamReader = new StreamReader(bindingContext.HttpContext.Request.Body);
        var json = await streamReader.ReadToEndAsync();

        try
        {
            var value = JsonSerializer.Deserialize<T>(json, JsonOptions)
                ?? throw new ArgumentException(bindingContext.FieldName); // TODO: test

            return await Validated<T>.CreateAsync(bindingContext.HttpContext.RequestServices, value);
        }
        catch (JsonException ex) when (ex.InnerException is InvalidOperationException castException)
        {
            return Validated<T>.CreateInvalid(new ValidationResult
            {
                Errors =
                {
                    new ValidationFailure(ex.Path, castException.Message)
                }
            });
        }
        catch (JsonException ex)
        {
            return Validated<T>.CreateInvalid(new ValidationResult
            {
                Errors =
                {
                    new ValidationFailure(ex.Path, ex.Message)
                }
            });
        }
        catch (Exception)
        {
            return Validated<T>.CreateInvalid(new ValidationResult
            {
                Errors =
                {
                    new ValidationFailure(bindingContext.FieldName, "Failed to parse model")
                }
            });
        }
    }
}
