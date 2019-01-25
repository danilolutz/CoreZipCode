using System.Net.Http;
using System.Text.RegularExpressions;
using CoreZipCode.Interfaces;

namespace CoreZipCode.Services.ViaCepApi
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
            try
            {
                value = value.Trim();
                if (string.IsNullOrEmpty(value) || value.Length < size)
                {
                    throw new ViaCepException($"Invalid {name} Param");
                }
            }
            catch (ViaCepException ex)
            {
                throw ex;
            }

            return value;
        }

        public static string ValidateZipCode(string zipcode)
        {
            zipcode = zipcode.Trim().Replace("-", "");
            try
            {
                if (zipcode.Length != 8)
                {
                    throw new ViaCepException(ZipCodeSizeErrorMessage);
                }
                if (!Regex.IsMatch(zipcode, ("[0-9]{8}")))
                {
                    throw new ViaCepException(ZipCodeFormatErrorMessage);
                }
            }
            catch (ViaCepException ex)
            {
                throw ex;
            }

            return zipcode;
        }
    }
}