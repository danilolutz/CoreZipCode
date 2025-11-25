using CoreZipCode.Interfaces;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CoreZipCode.Services.ZipCode.SmartyApi
{
    /// <summary>
    /// Provides access to SmartyStreets ZIP code and street address lookup services using authenticated API requests.
    /// </summary>
    /// <remarks>Use this class to construct authenticated requests to the SmartyStreets US ZIP code and
    /// street address APIs. The class requires valid authentication credentials and offers methods for building request
    /// URLs for ZIP code and address lookups. Instances can be initialized with authentication credentials alone, or
    /// with a custom HTTP client or API handler for advanced scenarios. This class is not thread-safe if credentials or
    /// handlers are modified externally.</remarks>
    public class Smarty : ZipCodeBaseService
    {
        private const string BaseZipcodeUrl = "https://us-zipcode.api.smartystreets.com/lookup";
        private const string BaseStreetUrl = "https://us-street.api.smartystreets.com/street-address";

        private readonly string _authId;
        private readonly string _authToken;

        /// <summary>
        /// Initializes a new instance of the Smarty class using the specified authentication credentials.
        /// </summary>
        /// <param name="authId">The authentication identifier used to authorize API requests. Cannot be null, empty, or consist only of
        /// white-space characters.</param>
        /// <param name="authToken">The authentication token used to authorize API requests. Cannot be null, empty, or consist only of
        /// white-space characters.</param>
        public Smarty(string authId, string authToken)
        {
            _authId = Validate.NotNullOrWhiteSpace(authId, nameof(authId));
            _authToken = Validate.NotNullOrWhiteSpace(authToken, nameof(authToken));
        }

        /// <summary>
        /// Initializes a new instance of the Smarty class using the specified HTTP client and authentication
        /// credentials.
        /// </summary>
        /// <param name="httpClient">The HTTP client instance used to send requests to the Smarty API. Must not be null.</param>
        /// <param name="authId">The authentication identifier used to authorize requests. Must not be null or whitespace.</param>
        /// <param name="authToken">The authentication token used to authorize requests. Must not be null or whitespace.</param>
        public Smarty(HttpClient httpClient, string authId, string authToken)
            : base(httpClient)
        {
            _authId = Validate.NotNullOrWhiteSpace(authId, nameof(authId));
            _authToken = Validate.NotNullOrWhiteSpace(authToken, nameof(authToken));
        }

        /// <summary>
        /// Initializes a new instance of the Smarty class using the specified API handler and authentication
        /// credentials.
        /// </summary>
        /// <param name="apiHandler">The API handler used to communicate with the Smarty service. Must not be null.</param>
        /// <param name="authId">The authentication identifier for accessing the Smarty API. Must not be null or whitespace.</param>
        /// <param name="authToken">The authentication token for accessing the Smarty API. Must not be null or whitespace.</param>
        public Smarty(IApiHandler apiHandler, string authId, string authToken)
            : base(apiHandler)
        {
            _authId = Validate.NotNullOrWhiteSpace(authId, nameof(authId));
            _authToken = Validate.NotNullOrWhiteSpace(authToken, nameof(authToken));
        }

        /// <summary>
        /// Constructs a URL for retrieving information related to the specified ZIP code, including authentication
        /// parameters.
        /// </summary>
        /// <param name="zipcode">The ZIP code to include in the URL. Must be a valid ZIP code format; otherwise, an exception may be thrown.</param>
        /// <returns>A string containing the fully constructed URL with authentication and ZIP code parameters.</returns>
        public override string SetZipCodeUrl(string zipcode)
            => $"{BaseZipcodeUrl}?auth-id={_authId}&auth-token={_authToken}&zipcode={ValidateZipCode(zipcode)}";

        public override string SetZipCodeUrlBy(string state, string city, string street)
            => $"{BaseStreetUrl}?auth-id={_authId}&auth-token={_authToken}" +
               $"&street={ValidateParam("Street", street)}" +
               $"&city={ValidateParam("City", city)}" +
               $"&state={ValidateParam("State", state, 32)}" +
               $"&candidates=10" +
               $"&match=enhanced";

        private static string ValidateParam(string name, string value, int maxSize = 64)
        {
            var trimmed = value.Trim();
            if (trimmed.Length > maxSize)
                throw new SmartyException($"Invalid {name}: exceeds {maxSize} characters.");
            return Uri.EscapeDataString(trimmed); // Smarty demands URL encoding
        }

        private static string ValidateZipCode(string zipCode)
        {
            var clean = zipCode.Trim().Replace("-", "");
            if (clean.Length < 5 || clean.Length > 16)
                throw new SmartyException("Invalid ZipCode Size");

            if (!Regex.IsMatch(clean, @"^\d{5,16}$"))
                throw new SmartyException("Invalid ZipCode Format");

            return clean;
        }

        private static class Validate
        {
            public static string NotNullOrWhiteSpace(string value, string paramName)
                => string.IsNullOrWhiteSpace(value)
                    ? throw new ArgumentNullException(paramName)
                    : value;
        }
    }
}
