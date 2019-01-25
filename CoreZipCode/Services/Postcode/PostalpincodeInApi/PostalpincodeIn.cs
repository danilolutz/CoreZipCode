using System.Net.Http;
using CoreZipCode.Interfaces;

namespace CoreZipCode.Services.Postcode.PostalpincodeInApi
{
    public class PostalpincodeIn : PostCodeBaseService
    {
        public PostalpincodeIn(HttpClient request) : base(request)
        {
            //
        }

        public override string SetPostCodeUrl(string postcode)
        {
            return $"http://postalpincode.in/api/pincode/{postcode}";
        }
    }
}