using System.Net.Http;
using CoreZipCode.Interfaces;

namespace CoreZipCode.Services.Postcode.PostcodesIoApi
{
    public class PostcodesIo : PostCodeBaseService
    {
        public PostcodesIo(HttpClient request) : base(request)
        {
            //
        }

        public override string SetPostCodeUrl(string postcode)
        {
            return $"https://api.postcodes.io/postcodes?q={postcode}";
        }
    }
}