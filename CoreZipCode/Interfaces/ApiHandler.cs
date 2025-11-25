using CoreZipCode.Result;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreZipCode.Interfaces
{
    /// <summary>
    /// ApiHandler class
    /// </summary>
    public class ApiHandler : IApiHandler
    {
        /// <summary>
        /// Http Client Request.
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// ApiHandler Constructor without parameter: request.
        /// </summary>
        public ApiHandler() : this(new HttpClient()) { }

        /// <summary>
        /// ApiHandler Constructor with parameter: request.
        /// </summary>
        /// <param name="httpClient">HttpClient class param to handle with API Servers Connections.</param>
        public ApiHandler(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Sends an asynchronous HTTP GET request to the specified URL and returns the response content or an error
        /// result. 
        /// </summary>
        /// <remarks>If the request fails due to network issues, timeouts, or a non-success HTTP status
        /// code, the returned result will contain an <see cref="ApiError"/> describing the failure. The method does not
        /// throw exceptions for typical HTTP or network errors; instead, errors are encapsulated in the result
        /// object.</remarks>
        /// <param name="url">The absolute URL of the API endpoint to call. Cannot be null, empty, or whitespace.</param>
        /// <returns>A <see cref="Result{string}"/> containing the response body if the request succeeds; otherwise, a failure
        /// result with error details.</returns>
        public virtual async Task<Result<string>> CallApiAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return Result<string>.Failure(
                    new ApiError(HttpStatusCode.BadRequest, "URL cannot be null or empty."));

            try
            {
                var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead)
                                                      .ConfigureAwait(false);

                var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                    return Result<string>.Success(body);

                var errorMessage = $"API returned {(int)response.StatusCode} {response.StatusCode}";
                var error = new ApiError(response.StatusCode, errorMessage, response.ReasonPhrase, body);

                return Result<string>.Failure(error);
            }
            catch (HttpRequestException ex)
            {
                return Result<string>.Failure(
                    new ApiError(HttpStatusCode.ServiceUnavailable, "Network or connection error.", ex.Message));
            }
            catch (TaskCanceledException ex)
            {
                return Result<string>.Failure(
                    new ApiError(HttpStatusCode.RequestTimeout, "Request timed out.", ex.Message));
            }
            catch (OperationCanceledException ex)
            {
                return Result<string>.Failure(
                    new ApiError(HttpStatusCode.BadRequest, "Request was cancelled.", ex.Message));
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(
                    new ApiError(HttpStatusCode.InternalServerError, "Unexpected error.", ex.Message));
            }
        }
    }
}