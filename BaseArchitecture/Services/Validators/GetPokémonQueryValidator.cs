namespace Services.Validators;

public class GetPokémonQueryValidator : AbstractValidator<GetPokémonQuery>
{
    public GetPokémonQueryValidator()
    {
        RuleFor(m => m.Name).NotEmpty().MaximumLength(100);
    }
}
