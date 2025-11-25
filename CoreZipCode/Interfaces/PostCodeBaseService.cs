using CoreZipCode.Result;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreZipCode.Interfaces
{
    /// <summary>
    /// Postcode base service abstract class.
    /// </summary>
    public abstract class PostcodeBaseService : IPostcodeService
    {
        private readonly IApiHandler _apiHandler;

        /// <summary>
        /// Initializes a new instance of the PostcodeBaseService class using a default ApiHandler. 
        /// </summary>
        /// <remarks>This protected constructor is intended for use by derived classes to provide a
        /// default API handler. For custom API handler configuration, use the constructor that accepts an ApiHandler
        /// parameter.</remarks>
        protected PostcodeBaseService()
            : this(new ApiHandler())
        {
        }

        /// <summary>
        /// Initializes a new instance of the PostcodeBaseService class using the specified HTTP client for API
        /// communication.
        /// </summary>
        /// <remarks>This constructor allows customization of the underlying HTTP client, enabling
        /// configuration of timeouts, headers, or other HTTP settings as needed for API interactions.</remarks>
        /// <param name="httpClient">The HTTP client used to send requests to the postcode API. Cannot be null.</param>
        protected PostcodeBaseService(HttpClient httpClient)
            : this(new ApiHandler(httpClient))
        {
        }

        /// <summary>
        /// Initializes a new instance of the PostcodeBaseService class with the specified API handler.
        /// </summary>
        /// <param name="apiHandler">The API handler used to perform requests to the underlying postcode service. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="apiHandler"/> is null.</exception>
        protected PostcodeBaseService(IApiHandler apiHandler)
        {
            _apiHandler = apiHandler ?? throw new ArgumentNullException(nameof(apiHandler));
        }

        /// <summary>
        /// Executes a synchronous postcode lookup and returns the result as a string. This method is obsolete; use
        /// GetPostcodeAsync<T> for improved error handling and asynchronous operation.
        /// </summary>
        /// <remarks>This method is obsolete and always throws a NotSupportedException. For better error
        /// handling and asynchronous support, use GetPostcodeAsync<T> instead.</remarks>
        /// <param name="postcode">The postcode to look up. Cannot be null or empty.</param>
        /// <returns>A string containing the result of the postcode lookup.</returns>
        /// <exception cref="NotSupportedException">Thrown in all cases. This method is not supported; use the asynchronous alternative.</exception>
        [Obsolete("Use GetPostcodeAsync<T> witch returns Result<T>  for better error handling.")]
        public virtual string Execute(string postcode) => throw new NotSupportedException("Use async version with Result.");

        /// <summary>
        /// Retrieves postcode information synchronously. This method is obsolete; use GetPostcodeAsync<T> instead.
        /// </summary>
        /// <remarks>This method is obsolete and always throws a NotSupportedException. Use
        /// GetPostcodeAsync<T>, which returns a Result<T>, for asynchronous operations.</remarks>
        /// <typeparam name="T">The type of the result returned for the postcode information.</typeparam>
        /// <param name="postcode">The postcode to retrieve information for. Cannot be null or empty.</param>
        /// <returns>The postcode information of type T.</returns>
        /// <exception cref="NotSupportedException">Thrown in all cases. This method is not supported; use the asynchronous version instead.</exception>
        [Obsolete("Use GetPostcodeAsync<T> witch returns Result<T>.")]
        public virtual T GetPostcode<T>(string postcode) => throw new NotSupportedException("Use async version with Result.");

        /// <summary>
        /// Asynchronously retrieves postcode information and returns the result as a strongly typed object.
        /// </summary>
        /// <remarks>If the postcode is not found or the API call fails, the returned result will indicate
        /// failure and include error information. This method is typically used to fetch and deserialize postcode data
        /// from an external API.</remarks>
        /// <typeparam name="T">The type of the object to deserialize the postcode information into. Must be a reference type.</typeparam>
        /// <param name="postcode">The postcode to query. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/> object
        /// with the postcode information if successful; otherwise, contains error details.</returns>
        public virtual async Task<Result<T>> GetPostcodeAsync<T>(string postcode) where T : class
        {
            var url = SetPostcodeUrl(postcode);
            var result = await _apiHandler.CallApiAsync(url);

            return await result.Match(
                onSuccess: body => HandleSuccess<T>(body),
                onFailure: error => Task.FromResult(Result<T>.Failure(error)));
        }

        /// <summary>
        /// Deserializes the specified JSON string into an object of type T and returns a successful result if parsing
        /// succeeds; otherwise, returns a failure result with an appropriate error.
        /// </summary>
        /// <remarks>If the JSON string cannot be parsed or does not contain a valid object of type T, the
        /// returned result will indicate failure and include error information. This method does not throw exceptions
        /// for invalid JSON; instead, errors are encapsulated in the result.</remarks>
        /// <typeparam name="T">The type of object to deserialize from the JSON string. Must be a reference type.</typeparam>
        /// <param name="json">The JSON string representing the object to deserialize. Cannot be null.</param>
        /// <returns>A Result<T> containing the deserialized object if successful; otherwise, a failure result with error details
        /// if the JSON is invalid or does not represent a valid object.</returns>
        private static async Task<Result<T>> HandleSuccess<T>(string json) where T : class
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(json);
                return obj is null
                    ? Result<T>.Failure(new ApiError(System.Net.HttpStatusCode.NotFound, "Postcode not found or empty response."))
                    : Result<T>.Success(obj);
            }
            catch (JsonException ex)
            {
                return Result<T>.Failure(new ApiError(
                    System.Net.HttpStatusCode.UnprocessableEntity,
                    "Failed to parse postcode API response.",
                    ex.Message,
                    json));
            }
        }

        /// <summary>
        /// Generates a URL for retrieving information related to the specified postcode.
        /// </summary>
        /// <param name="postcode">The postcode for which to generate the URL. Cannot be null or empty.</param>
        /// <returns>A string containing the URL associated with the provided postcode.</returns>
        public abstract string SetPostcodeUrl(string postcode);
    }
}
