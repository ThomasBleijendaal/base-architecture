using FluentValidation;

namespace API.Models;

public class LikePokémonRequestModelValidator : AbstractValidator<LikePokémonRequestModel>
{
    public LikePokémonRequestModelValidator()
    {
        RuleFor(m => m.Id).InclusiveBetween(1, 9999);
    }
}
