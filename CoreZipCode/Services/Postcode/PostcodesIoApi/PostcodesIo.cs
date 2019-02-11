using CoreZipCode.Interfaces;
using System.Net.Http;

namespace CoreZipCode.Services.Postcode.PostcodesIoApi
{
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