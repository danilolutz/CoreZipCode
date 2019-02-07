using System;
using System.Net.Http;
using CoreZipCode.Interfaces;

namespace CoreZipCode.Services.ZipCode.SmartyStreetsApi
{
    public class SmartyStreets : ZipCodeBaseService
    {
        private readonly string _authID;
        private readonly string _authToken;
        private readonly string _baseUrl;

        public SmartyStreets(HttpClient request, string authID, string authToken) : base(request)
        {
            _authID = authID ?? throw new ArgumentNullException(nameof(authID));
            _authToken = authToken ?? throw new ArgumentNullException(nameof(authToken));

            _baseUrl = $"https://us-zipcode.api.smartystreets.com/lookup?auth-id={_authID}&auth-token={_authToken}";
        }

        public override string SetZipCodeUrl(string zipcode) => $"{_baseUrl}&zipcode={zipcode}";

        public override string SetZipCodeUrlBy(string state, string city, string street) => $"{_baseUrl}&street={street}&city={city}&state={state}&candidates=10";
    }
}