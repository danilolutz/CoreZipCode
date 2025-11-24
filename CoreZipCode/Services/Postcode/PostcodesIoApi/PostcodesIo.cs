using CoreZipCode.Interfaces;
using System.Net.Http;

namespace CoreZipCode.Services.Postcode.PostcodesIoApi
{
    /// <summary>
    /// Provides access to postcode lookup services using the Postcodes.io API.
    /// </summary>
    /// <remarks>This class extends <see cref="PostcodeBaseService"/>. Use this class to construct requests for postcode 
    /// information via the postcodes.io endpoint. Inherit from this class to extend or customize postcode-related operations.
    /// This class is intended for use with UK postcodes and relies on an HTTP client for making API requests. For more 
    /// information about the Postcodes API, see https://postcodes.io.</remarks>
    public class PostcodesIo : PostcodeBaseService
    {
        public PostcodesIo() { }

        public PostcodesIo(HttpClient request) : base(request)
        {
            //
        }

        public override string SetPostcodeUrl(string postcode)
        {
            return $"https://api.postcodes.io/postcodes?q={postcode}";
        }
    }
}