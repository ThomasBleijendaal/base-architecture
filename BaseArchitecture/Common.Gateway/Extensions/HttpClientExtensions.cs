using System.Text.Json;

namespace Common.Gateway.Extensions;

// TODO: do a try catch to handle exceptions

public static class HttpClientExtensions
{
    public static async Task<Response> GetResultFromJsonAsync<T>(this HttpClient httpClient, Request request)
    {
        var response = await httpClient.GetAsync(request.Uri);

        return new Response(response.IsSuccessStatusCode || request.SuccessCodes.Contains(response.StatusCode));
    }

    public static async Task<Response<T>> GetResultFromJsonAsync<T>(this HttpClient httpClient, Request<T> request)
    {
        var response = await httpClient.GetAsync(request.Uri);

        if (!response.IsSuccessStatusCode)
        {
            return new Response<T>(request.SuccessCodes.Contains(response.StatusCode), default);
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        var responseObject = JsonSerializer.Deserialize<T>(responseContent);

        return new Response<T>(true, responseObject);
    }

    public static async Task<Response<TSuccess, TError>> GetResultFromJsonAsync<TSuccess, TError>(this HttpClient httpClient, Request<TSuccess, TError> request)
    {
        var response = await httpClient.GetAsync(request.Uri);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (request.ErrorCodes.Contains(response.StatusCode))
            {
                var responseObject = JsonSerializer.Deserialize<TError>(responseContent);

                return new Response<TSuccess, TError>(false, default, responseObject);
            }
            else
            {
                return new Response<TSuccess, TError>(request.SuccessCodes.Contains(response.StatusCode), default, default);
            }
        }
        else
        {
            var responseObject = JsonSerializer.Deserialize<TSuccess>(responseContent);

            return new Response<TSuccess, TError>(true, responseObject, default);
        }
    }
}
