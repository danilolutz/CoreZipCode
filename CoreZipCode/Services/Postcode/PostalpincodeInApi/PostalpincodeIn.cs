using CoreZipCode.Interfaces;
using System.Net.Http;

namespace CoreZipCode.Services.Postcode.PostalpincodeInApi
{
    /// <summary>
    /// Provides access to the Postalpincode.in API for retrieving postal code information.
    /// </summary>
    /// <remarks>This class extends <see cref="PostcodeBaseService"/> to implement API-specific behavior for
    /// Postalpincode.in. Use this class to construct requests and obtain data related to Indian postal pincodes.
    /// Instances can be initialized with an <see cref="HttpClient"/> to customize HTTP request handling. 
    /// For more information about the Postal Pin Code API, see http://www.postalpincode.in/.</remarks>
    public class PostalpincodeIn : PostcodeBaseService
    {
        public PostalpincodeIn() { }

        public PostalpincodeIn(HttpClient httpClient) : base(httpClient) { }

        public PostalpincodeIn(IApiHandler apiHandler) : base(apiHandler) { }

        public override string SetPostcodeUrl(string postcode)
            => $"http://postalpincode.in/api/pincode/{postcode.Trim()}";
    }
}