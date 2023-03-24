namespace Common.Exceptions;

public class TransientException : ResultException
{
    public TransientException(IEnumerable<ResultError> resultErrors) : base(resultErrors)
    {
    }
}
