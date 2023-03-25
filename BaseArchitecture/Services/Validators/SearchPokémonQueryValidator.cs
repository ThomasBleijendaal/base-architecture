namespace Services.Validators;

public class SearchPokémonQueryValidator : AbstractValidator<SearchPokémonQuery>
{
    public SearchPokémonQueryValidator()
    {
        RuleFor(m => m.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Height).InclusiveBetween(0, 1000);
        RuleFor(x => x.Weight).InclusiveBetween(0, 1000);
    }
}
