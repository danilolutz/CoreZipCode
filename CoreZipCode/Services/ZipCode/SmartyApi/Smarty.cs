using CoreZipCode.Interfaces;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CoreZipCode.Services.ZipCode.SmartyApi
{
    /// <summary>
    /// Provides access to Smarty ZIP code and street address lookup services using authenticated API requests.
    /// </summary>
    /// <remarks>Use this class to construct authenticated requests to the Smarty US ZIP code and
    /// street address APIs. The class requires valid authentication credentials, which are used to authorize each
    /// request. Inherit from <see cref="ZipCodeBaseService"/> to enable integration with other ZIP code service implementations. This
    /// class is not thread-safe; create a separate instance for each concurrent operation if needed. For more information about the Smarty API, see
    /// https://www.smarty.com/.</remarks>
    public class Smarty : ZipCodeBaseService
    {
        private const string ZipCodeSizeErrorMessage = "Invalid ZipCode Size";
        private const string ZipCodeFormatErrorMessage = "Invalid ZipCode Format";

        private const string BaseZipcodeUrl = "https://us-zipcode.api.smartystreets.com/lookup";
        private const string BaseStreetUrl = "https://us-street.api.smartystreets.com/street-address";

        private readonly string _authId;
        private readonly string _authToken;

        public Smarty(string authId, string authToken)
        {
            _authId = string.IsNullOrWhiteSpace(authId) ? throw new ArgumentNullException(nameof(authId)) : authId;
            _authToken = string.IsNullOrWhiteSpace(authToken) ? throw new ArgumentNullException(nameof(authToken)) : authToken;
        }

        public Smarty(HttpClient request, string authId, string authToken) : base(request)
        {
            _authId = string.IsNullOrWhiteSpace(authId) ? throw new ArgumentNullException(nameof(authId)) : authId;
            _authToken = string.IsNullOrWhiteSpace(authToken) ? throw new ArgumentNullException(nameof(authToken)) : authToken;
        }

        public override string SetZipCodeUrl(string zipcode) => $"{BaseZipcodeUrl}?auth-id={_authId}&auth-token={_authToken}&zipcode={ValidateZipCode(zipcode)}";

        public override string SetZipCodeUrlBy(string state, string city, string street) => $"{BaseStreetUrl}?auth-id={_authId}&auth-token={_authToken}&street={ValidateParam("Street", street)}&city={ValidateParam("City", city)}&state={ValidateParam("State", state, 32)}&candidates=10";

        private static string ValidateParam(string name, string value, int size = 64)
        {
            var aux = value;
            if (aux.Length > size)
            {
                throw new SmartyException($"Invalid {name}, parameter over size of {size} characters.");
            }

            return aux.Trim();
        }

        private static string ValidateZipCode(string zipCode)
        {
            var zipAux = zipCode.Trim().Replace("-", "");

            if (zipAux.Length < 5 || zipAux.Length > 16)
            {
                throw new SmartyException(ZipCodeSizeErrorMessage);
            }

            if (!Regex.IsMatch(zipAux, ("[0-9]{5,16}")))
            {
                throw new SmartyException(ZipCodeFormatErrorMessage);
            }

            return zipAux;
        }
    }
}