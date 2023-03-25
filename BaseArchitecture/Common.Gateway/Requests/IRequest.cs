namespace Common.Gateway.Requests;

public interface IRequest
{
    HttpMethod HttpMethod { get; }
    FormattableString Uri { get; }
    BinaryData? Content { get; }
    string GetUrlSafeUri();
}
