using FluentValidation;

namespace API.Models;

public class GetPokémonRequestModelValidator : AbstractValidator<GetPokémonRequestModel>
{
    public GetPokémonRequestModelValidator()
    {
        RuleFor(m => m.Name).NotEmpty();
    }
}
