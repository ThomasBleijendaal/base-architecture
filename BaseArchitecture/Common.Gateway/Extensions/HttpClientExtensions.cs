namespace Common.Gateway.Extensions;

public static class HttpClientExtensions
{
    public static async Task<Response> SendAsync<T>(this HttpClient httpClient, Request request)
    {
        try
        {
            var response = await SendRequestAsync(httpClient, request);

            return new Response(response.IsSuccessStatusCode || request.SuccessCodes.Contains(response.StatusCode));
        }
        catch (Exception ex)
        {
            return new Response(false)
            {
                Exception = ex
            };
        }
    }

    public static async Task<Response<T>> SendAsync<T>(this HttpClient httpClient, Request<T> request, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        try
        {
            var response = await SendRequestAsync(httpClient, request);

            if (!response.IsSuccessStatusCode)
            {
                return new Response<T>(request.SuccessCodes.Contains(response.StatusCode), default);
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var responseObject = JsonSerializer.Deserialize<T>(responseContent, jsonSerializerOptions);

            return new Response<T>(true, responseObject);
        }
        catch (Exception ex)
        {
            return new Response<T>(false, default)
            {
                Exception = ex
            };
        }
    }

    public static async Task<Response<TSuccess, TError>> SendAsync<TSuccess, TError>(this HttpClient httpClient, Request<TSuccess, TError> request, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        try
        {
            var response = await SendRequestAsync(httpClient, request);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                if (request.ErrorCodes.Contains(response.StatusCode))
                {
                    var responseObject = JsonSerializer.Deserialize<TError>(responseContent, jsonSerializerOptions);

                    return new Response<TSuccess, TError>(false, default, responseObject);
                }
                else
                {
                    return new Response<TSuccess, TError>(request.SuccessCodes.Contains(response.StatusCode), default, default);
                }
            }
            else
            {
                var responseObject = JsonSerializer.Deserialize<TSuccess>(responseContent, jsonSerializerOptions);

                return new Response<TSuccess, TError>(true, responseObject, default);
            }
        }
        catch (Exception ex)
        {
            return new Response<TSuccess, TError>(false, default, default)
            {
                Exception = ex
            };
        }
    }

    private static async Task<HttpResponseMessage> SendRequestAsync(HttpClient httpClient, IRequest request)
    {
        var requestMessage = new HttpRequestMessage(request.HttpMethod, request.GetUrlSafeUri());

        if (request.Content != null)
        {
            requestMessage.Content = new StreamContent(request.Content.ToStream());
        }

        var response = await httpClient.SendAsync(requestMessage);

        return response;
    }
}
