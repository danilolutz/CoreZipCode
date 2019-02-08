using CoreZipCode.Interfaces;
using System;
using System.Net.Http;

namespace CoreZipCode.Services
{
    [Obsolete("This class was deprecated since version 1.1.0 and will be removed in next version, please use Services.ViaCepApi.ViaCep", false)]
    public class ViaCep : ZipCodeBaseService
    {
        public ViaCep() : base(new HttpClient())
        {
            //
        }
        public ViaCep(HttpClient request) : base(request)
        {
            //
        }
        public override string SetZipCodeUrl(string zipCode)
        {
            zipCode = zipCode.Replace("-", "");
            return $"https://viacep.com.br/ws/{zipCode}/json/";
        }

        public override string SetZipCodeUrlBy(string state, string city, string street) => $"https://viacep.com.br/ws/{state}/{city}/{street}/json/";
    }
}