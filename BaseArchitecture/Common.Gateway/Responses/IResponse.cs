namespace Common.Gateway.Responses;

public interface IResponse
{
    public bool Success { get; }
    public Exception? Exception { get; }
}
