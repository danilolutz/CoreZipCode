using CoreZipCode.Interfaces;
using System.Net.Http;

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