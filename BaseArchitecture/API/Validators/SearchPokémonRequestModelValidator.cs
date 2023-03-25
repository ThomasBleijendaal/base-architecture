using FluentValidation;

namespace API.Models;

public class SearchPokémonRequestModelValidator : AbstractValidator<SearchPokémonRequestModel>
{
    public SearchPokémonRequestModelValidator()
    {
        RuleFor(x => x.Name).MaximumLength(100);
        RuleFor(x => x.Height).InclusiveBetween(0, 1000);
        RuleFor(x => x.Weight).InclusiveBetween(0, 1000);
    }
}
