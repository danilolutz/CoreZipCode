using CoreZipCode.Interfaces;
using System.Net.Http;

namespace CoreZipCode.Services.Postcode.PostalpincodeInApi
{
    public class PostalpincodeIn : PostcodeBaseService
    {
        public PostalpincodeIn() { }

        public PostalpincodeIn(HttpClient request) : base(request)
        {
            //
        }

        public override string SetPostcodeUrl(string postcode)
        {
            return $"http://postalpincode.in/api/pincode/{postcode}";
        }
    }
}