namespace Services.Validators;

public class LikePokémonCommandValidator : AbstractValidator<LikePokémonCommand>
{
    public LikePokémonCommandValidator()
    {
        RuleFor(m => m.Id).InclusiveBetween(1, 9990);
    }
}
