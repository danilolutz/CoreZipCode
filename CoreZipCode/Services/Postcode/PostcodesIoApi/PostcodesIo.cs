using CoreZipCode.Interfaces;
using System.Net.Http;

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