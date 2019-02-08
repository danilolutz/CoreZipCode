using System;
using System.Net.Http;
using CoreZipCode.Interfaces;
using System.Text.RegularExpressions;

namespace CoreZipCode.Services.ZipCode.SmartyStreetsApi
{
    public class SmartyStreets : ZipCodeBaseService
    {
        private const string ZipCodeSizeErrorMessage = "Invalid ZipCode Size";
        private const string ZipCodeFormatErrorMessage = "Invalid ZipCode Format";
        private readonly string _baseUrl;

        public SmartyStreets(HttpClient request, string authId, string authToken) : base(request)
        {
            if (string.IsNullOrWhiteSpace(authId))
                throw new ArgumentNullException(nameof(authId));

            if (string.IsNullOrWhiteSpace(authToken))
                throw new ArgumentNullException(nameof(authToken));

            _baseUrl = $"https://us-zipcode.api.smartystreets.com/lookup?auth-id={authId}&auth-token={authToken}";
        }

        public override string SetZipCodeUrl(string zipcode) => $"{_baseUrl}&zipcode={ValidateZipCode(zipcode)}";

        public override string SetZipCodeUrlBy(string state, string city, string street) => $"{_baseUrl}&street={ValidateParam("Street", street, 64)}&city={ValidateParam("City", city, 64)}&state={ValidateParam("State", state, 32)}&candidates=10";

        private static string ValidateParam(string name, string value, int size = 16)
        {
            if (value.Length > size)
            {
                throw new SmartyStreetsException($"Invalid {name}, parameter over size");
            }

            return value.Trim();
        }

        private static string ValidateZipCode(string zipCode)
        {
            zipCode = zipCode.Trim().Replace("-", "");

            if (zipCode.Length < 5 || zipCode.Length > 16)
            {
                throw new SmartyStreetsException(ZipCodeSizeErrorMessage);
            }

            if (!Regex.IsMatch(zipCode, ("[0-9]{5,16}")))
            {
                throw new SmartyStreetsException(ZipCodeFormatErrorMessage);
            }

            return zipCode;
        }
    }
}