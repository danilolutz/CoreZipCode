using System;
using CoreZipCode.Interfaces;

namespace CoreZipCode.Services
{
    [Obsolete("This class was deprecated since version 1.2.0 and will be removed in next version, please use Services.ViaCepApi.ViaCep", false)]
    public class ViaCep : ZipCodeBaseService
    {
        public override string SetZipCodeUrl(string zipcode)
        {
            zipcode = zipcode.Replace("-", "");
            return $"https://viacep.com.br/ws/{zipcode}/json/";
        }

        public override string SetZipCodeUrlBy(string state, string city, string street) => $"https://viacep.com.br/ws/{state}/{city}/{street}/json/";
    }
}