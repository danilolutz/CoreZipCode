using CoreZipCode.Interfaces;

namespace CoreZipCode.Services
{
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