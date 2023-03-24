namespace Common.Exceptions;

public class ResultException : Exception
{
    public ResultException(IEnumerable<ResultError> resultErrors)
    {
        ResultErrors = resultErrors;
    }

    public IEnumerable<ResultError> ResultErrors { get; }
}
