using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace API.Models;

public class GetPokémonsRequestModelValidator : AbstractValidator<GetPokémonsRequestModel>
{
    public GetPokémonsRequestModelValidator()
    {
        RuleFor(m => m.Level).GreaterThanOrEqualTo(0).LessThanOrEqualTo(1);
    }
}

public class ValidatedObjectModelValidator : IObjectModelValidator
{
    public void Validate(ActionContext actionContext, ValidationStateDictionary? validationState, string prefix, object? model)
    {

    }
}
