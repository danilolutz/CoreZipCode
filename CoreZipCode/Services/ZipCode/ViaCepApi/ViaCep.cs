using CoreZipCode.Interfaces;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CoreZipCode.Services.ZipCode.ViaCepApi
{
    public class ViaCep : ZipCodeBaseService
    {
        private const string ZipCodeSizeErrorMessage = "Invalid ZipCode Size";
        private const string ZipCodeFormatErrorMessage = "Invalid ZipCode Format";

        public ViaCep(HttpClient request) : base(request)
        {
            //
        }

        public override string SetZipCodeUrl(string zipcode) => $"https://viacep.com.br/ws/{ValidateZipCode(zipcode)}/json/";

        public override string SetZipCodeUrlBy(string state, string city, string street) => $"https://viacep.com.br/ws/{ValidateParam("State", state, 2)}/{ValidateParam("City", city)}/{ValidateParam("Street", street)}/json/";

        public static string ValidateParam(string name, string value, int size = 3)
        {
            value = value.Trim();

            if (string.IsNullOrEmpty(value) || value.Length < size)
            {
                throw new ViaCepException($"Invalid {name} Param");
            }

            return value;
        }

        public static string ValidateZipCode(string zipcode)
        {
            zipcode = zipcode.Trim().Replace("-", "");

            if (zipcode.Length != 8)
            {
                throw new ViaCepException(ZipCodeSizeErrorMessage);
            }

            if (!Regex.IsMatch(zipcode, ("[0-9]{8}")))
            {
                throw new ViaCepException(ZipCodeFormatErrorMessage);
            }

            return zipcode;
        }
    }
}