using CoreZipCode.Result;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreZipCode.Interfaces
{
    /// <summary>
    /// Zip code base service abstract class
    /// </summary>
    public abstract class ZipCodeBaseService : IZipCodeService
    {
        private readonly IApiHandler _apiHandler;

        
        /// <summary>
        /// Initializes a new instance of the ZipCodeBaseService class using a default ApiHandler.
        /// </summary>
        /// <remarks>This protected constructor is intended for use by derived classes to provide a
        /// default configuration. It creates the service with a new instance of ApiHandler, which handles API
        /// communication. For custom API handler configurations, use the constructor that accepts an ApiHandler
        /// parameter.</remarks>
        protected ZipCodeBaseService()
            : this(new ApiHandler())
        {
        }

        /// <summary>
        /// Initializes a new instance of the ZipCodeBaseService class using the specified HTTP client for API
        /// communication.
        /// </summary>
        /// <remarks>This constructor allows for custom configuration of the underlying HTTP client, such
        /// as setting default headers or timeouts, before making API requests.</remarks>
        /// <param name="httpClient">The HTTP client used to send requests to the ZipCodeBase API. Cannot be null.</param>
        protected ZipCodeBaseService(HttpClient httpClient)
            : this(new ApiHandler(httpClient))
        {
        }

        /// <summary>
        /// Initializes a new instance of the ZipCodeBaseService class with the specified API handler.
        /// </summary>
        /// <param name="apiHandler">The API handler used to perform requests to the underlying ZIP code service. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="apiHandler"/> is null.</exception>
        protected ZipCodeBaseService(IApiHandler apiHandler)
        {
            _apiHandler = apiHandler ?? throw new ArgumentNullException(nameof(apiHandler));
        }

        /// <summary>
        /// Executes a synchronous address lookup for the specified ZIP code. This method is obsolete; use
        /// GetAddressAsync<T> with Result<T> for improved error handling.
        /// </summary>
        /// <remarks>Calling this method will always throw a NotSupportedException. For address lookups,
        /// use GetAddressAsync<T> with Result<T> to handle errors more effectively.</remarks>
        /// <param name="zipCode">The ZIP code to use for the address lookup. Must be a valid, non-empty string.</param>
        /// <returns>A string containing the address information associated with the specified ZIP code.</returns>
        /// <exception cref="NotSupportedException">Always thrown. This method is not supported; use the asynchronous alternative.</exception>
        [Obsolete("Use GetAddressAsync<T> witch returns Result<T> for better error handling.")]
        public virtual string Execute(string zipCode) => throw new NotSupportedException("Use async version with Result.");

        /// <summary>
        /// Retrieves the address information for the specified ZIP code. This method is obsolete; use
        /// GetAddressAsync<T> with Result<T> instead.
        /// </summary>
        /// <remarks>This method is obsolete and not supported. For address retrieval, use
        /// GetAddressAsync<T> with Result<T> to obtain results asynchronously.</remarks>
        /// <typeparam name="T">The type of the address result to return.</typeparam>
        /// <param name="zipCode">The ZIP code for which to retrieve address information. Cannot be null or empty.</param>
        /// <returns>The address information of type T corresponding to the specified ZIP code.</returns>
        /// <exception cref="NotSupportedException">Always thrown. This method is not supported; use the asynchronous version instead.</exception>
        [Obsolete("Use GetAddressAsync<T> witch returns Result<T>.")]
        public virtual T GetAddress<T>(string zipCode) => throw new NotSupportedException("Use async version with Result.");

        /// <summary>
        /// Asynchronously retrieves address information for the specified ZIP code and returns the result as an object
        /// of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the address result object. Must be a reference type.</typeparam>
        /// <param name="zipCode">The ZIP code for which to retrieve address information. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/> object
        /// with the address information if successful; otherwise, contains error details.</returns>
        public virtual async Task<Result<T>> GetAddressAsync<T>(string zipCode) where T : class
        {
            var url = SetZipCodeUrl(zipCode);
            var result = await _apiHandler.CallApiAsync(url);

            return await result.Match(
                onSuccess: body => HandleSuccess<T>(body),
                onFailure: error => Task.FromResult(Result<T>.Failure(error)));
        }

        /// <summary>
        /// Asynchronously retrieves a list of address records matching the specified state, city, and street.
        /// </summary>
        /// <remarks>If no addresses match the specified criteria, the returned list will be empty. The
        /// operation may fail if the API is unreachable or returns an error.</remarks>
        /// <typeparam name="T">The type of address record to return. Must be a reference type.</typeparam>
        /// <param name="state">The state to filter addresses by. Cannot be null or empty.</param>
        /// <param name="city">The city to filter addresses by. Cannot be null or empty.</param>
        /// <param name="street">The street to filter addresses by. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{IList{T}}"/>
        /// with the list of matching address records if successful; otherwise, contains error information.</returns>
        public virtual async Task<Result<IList<T>>> ListAddressesAsync<T>(string state, string city, string street) where T : class
        {
            var url = SetZipCodeUrlBy(state, city, street);
            var result = await _apiHandler.CallApiAsync(url);

            return await result.Match(
                onSuccess: body => HandleSuccessList<T>(body),
                onFailure: error => Task.FromResult(Result<IList<T>>.Failure(error)));
        }

        /// <summary>
        /// Deserializes the specified JSON string into an object of type <typeparamref name="T"/> and returns a
        /// successful result if parsing succeeds; otherwise, returns a failure result with an appropriate error.
        /// </summary>
        /// <remarks>If the JSON string cannot be parsed or does not represent a valid object of type
        /// <typeparamref name="T"/>, the returned result will indicate failure and include error details. The method
        /// returns a failure result with a 'NotFound' error if deserialization yields null, or an 'UnprocessableEntity'
        /// error if parsing fails due to invalid JSON.</remarks>
        /// <typeparam name="T">The type to deserialize the JSON string into. Must be a reference type.</typeparam>
        /// <param name="json">The JSON string representing the object to deserialize. Cannot be null.</param>
        /// <returns>A <see cref="Result{T}"/> containing the deserialized object if successful; otherwise, a failure result with
        /// an error describing the issue.</returns>
        private static async Task<Result<T>> HandleSuccess<T>(string json) where T : class
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(json);
                return obj is null
                    ? Result<T>.Failure(new ApiError(HttpStatusCode.NotFound, "Address not found or empty response."))
                    : Result<T>.Success(obj);
            }
            catch (JsonException ex)
            {
                return Result<T>.Failure(new ApiError(
                    HttpStatusCode.UnprocessableEntity,
                    "Failed to parse API response.",
                    ex.Message,
                    json));
            }
        }

        /// <summary>
        /// Deserializes the specified JSON string into a list of objects of type T and returns a success result. If
        /// deserialization fails, returns a failure result containing error details.
        /// </summary>
        /// <remarks>If the JSON string is valid but empty or null, an empty list is returned as a
        /// successful result. If the JSON is invalid or cannot be parsed, the result contains an error with details
        /// about the failure.</remarks>
        /// <typeparam name="T">The type of objects to deserialize from the JSON string. Must be a reference type.</typeparam>
        /// <param name="json">The JSON string representing a list of objects to deserialize. Cannot be null.</param>
        /// <returns>A Result containing the deserialized list of objects of type T if successful; otherwise, a failure result
        /// with error information.</returns>
        private static async Task<Result<IList<T>>> HandleSuccessList<T>(string json) where T : class
        {
            try
            {
                var list = JsonConvert.DeserializeObject<IList<T>>(json) ?? new List<T>();
                return Result<IList<T>>.Success(list);
            }
            catch (JsonException ex)
            {
                return Result<IList<T>>.Failure(new ApiError(
                    HttpStatusCode.UnprocessableEntity,
                    "Failed to parse address list.",
                    ex.Message,
                    json));
            }
        }

        /// <summary>
        /// Asynchronously retrieves the address information associated with the specified ZIP code.
        /// </summary>
        /// <remarks>This method is obsolete. Prefer using GetAddressAsync<T> that returns Result<T> for
        /// improved error handling and type safety.</remarks>
        /// <param name="zipCode">The ZIP code for which to retrieve address information. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the address information as a
        /// string.</returns>
        /// <exception cref="HttpRequestException">Thrown if the API request fails or returns an error response.</exception>
        [Obsolete("Prefer using GetAddressAsync<T> that returns Result<T>.")]
        public virtual async Task<string> ExecuteAsync(string zipCode)
        {
            var result = await _apiHandler.CallApiAsync(SetZipCodeUrl(zipCode));
            return result.Match(s => s, e => throw new HttpRequestException(e.Message));
        }

        /// <summary>
        /// Generates a URL for accessing information related to the specified ZIP code.
        /// </summary>
        /// <param name="zipCode">The ZIP code for which to generate the URL. Must be a valid postal code; cannot be null or empty.</param>
        /// <returns>A string containing the URL associated with the provided ZIP code.</returns>
        public abstract string SetZipCodeUrl(string zipCode);

        /// <summary>
        /// Generates a URL for retrieving ZIP code information based on the specified state, city, and street.
        /// </summary>
        /// <remarks>The format of the returned URL may vary depending on the implementation. Callers
        /// should ensure that all parameters are valid and properly formatted to avoid errors.</remarks>
        /// <param name="state">The two-letter abbreviation or full name of the state for which to generate the ZIP code URL. Cannot be null
        /// or empty.</param>
        /// <param name="city">The name of the city for which to generate the ZIP code URL. Cannot be null or empty.</param>
        /// <param name="street">The name of the street for which to generate the ZIP code URL. Cannot be null or empty.</param>
        /// <returns>A string containing the URL to access ZIP code information for the specified location.</returns>
        public abstract string SetZipCodeUrlBy(string state, string city, string street);
    }
}