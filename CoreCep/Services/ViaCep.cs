using CoreCep.Interfaces;

namespace CoreCep.Services
{
    public class ViaCep : ZipCodeBaseService
    {
        public override string SetZipCodeUrl(string zipcode)
        {
            zipcode = zipcode.Replace("-", "");
            return $"https://viacep.com.br/ws/{zipcode}/json/";    
        }

        public override string SetZipCodeUrlBy(string uf, string city, string street) => $"https://viacep.com.br/ws/{uf}/{city}/{street}/json/";
    }
}