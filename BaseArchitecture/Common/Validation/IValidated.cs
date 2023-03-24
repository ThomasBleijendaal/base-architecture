namespace Common.Validation;

public interface IValidated
{
    bool IsValid { get; }
    IEnumerable<ValidationError> Errors { get; }
}
