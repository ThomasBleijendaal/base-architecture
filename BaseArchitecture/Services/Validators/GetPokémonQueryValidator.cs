namespace Services.Validators;

public class GetPokémonQueryValidator : AbstractValidator<GetPokémonQuery>
{
    public GetPokémonQueryValidator()
    {
        RuleFor(m => m.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
    }
}
