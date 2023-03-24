namespace Common.Gateway.Extensions;

public static class HttpClientExtensions
{
    private readonly static JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true
    };

    public static async Task<Response> GetResultFromJsonAsync<T>(this HttpClient httpClient, Request request)
    {
        try
        {
            var response = await httpClient.GetAsync(request.GetUrlSafeUri());

            return new Response(response.IsSuccessStatusCode || request.SuccessCodes.Contains(response.StatusCode));
        }
        catch
        {
            return new Response(false);
        }
    }

    public static async Task<Response<T>> GetResultFromJsonAsync<T>(this HttpClient httpClient, Request<T> request)
    {
        try
        {
            var response = await httpClient.GetAsync(request.GetUrlSafeUri());

            if (!response.IsSuccessStatusCode)
            {
                return new Response<T>(request.SuccessCodes.Contains(response.StatusCode), default);
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var responseObject = JsonSerializer.Deserialize<T>(responseContent, JsonSerializerOptions);

            return new Response<T>(true, responseObject);
        }
        catch
        {
            return new Response<T>(false, default);
        }
    }

    public static async Task<Response<TSuccess, TError>> GetResultFromJsonAsync<TSuccess, TError>(this HttpClient httpClient, Request<TSuccess, TError> request)
    {
        try
        {
            var response = await httpClient.GetAsync(request.GetUrlSafeUri());

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                if (request.ErrorCodes.Contains(response.StatusCode))
                {
                    var responseObject = JsonSerializer.Deserialize<TError>(responseContent, JsonSerializerOptions);

                    return new Response<TSuccess, TError>(false, default, responseObject);
                }
                else
                {
                    return new Response<TSuccess, TError>(request.SuccessCodes.Contains(response.StatusCode), default, default);
                }
            }
            else
            {
                var responseObject = JsonSerializer.Deserialize<TSuccess>(responseContent, JsonSerializerOptions);

                return new Response<TSuccess, TError>(true, responseObject, default);
            }
        }
        catch
        {
            return new Response<TSuccess, TError>(false, default, default);
        }
    }
}
