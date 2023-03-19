namespace Common.Validation;

public interface IValidated
{
    bool IsValid { get; }
    IEnumerable<ValidationFailure> Errors { get; }
}
