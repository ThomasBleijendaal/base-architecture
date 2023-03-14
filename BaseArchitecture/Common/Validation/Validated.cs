using System.Reflection;
using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Validation;

public class Validated<T>
{
    private ValidationResult Validation { get; }

    private Validated(T value, ValidationResult validation)
    {
        Value = value;
        Validation = validation;
    }

    public T Value { get; }
    public bool IsValid => Validation.IsValid;

    public IDictionary<string, string[]> Errors =>
        Validation
            .Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

    public void Deconstruct(out bool isValid, out T value)
    {
        isValid = IsValid;
        value = Value;
    }

    public static async ValueTask<Validated<T>> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        using var streamReader = new StreamReader(context.Request.Body);

        var value = JsonSerializer.Deserialize<T>(await streamReader.ReadToEndAsync());
        var validator = context.RequestServices.GetRequiredService<IValidator<T>>();

        if (value is null)
        {
            throw new ArgumentException(parameter.Name);
        }

        var results = await validator.ValidateAsync(value);

        return new Validated<T>(value, results);
    }
}
