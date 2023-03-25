namespace Services.Validators;

public class GetPokémonsQueryValidator : AbstractValidator<GetPokémonsQuery>
{
    public GetPokémonsQueryValidator()
    {
        RuleFor(m => m.Type).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
    }
}
