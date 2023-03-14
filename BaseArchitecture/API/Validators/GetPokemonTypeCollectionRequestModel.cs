namespace API.Models;

public class GetPokémonTypeCollectionRequestModelValidator : AbstractValidator<GetPokémonTypeCollectionRequestModel>
{
    public GetPokémonTypeCollectionRequestModelValidator()
    {
        RuleFor(m => m.Level).GreaterThanOrEqualTo(0).LessThanOrEqualTo(1);
    }
}
