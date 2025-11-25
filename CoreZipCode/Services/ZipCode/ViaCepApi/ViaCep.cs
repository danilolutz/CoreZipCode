using CoreZipCode.Interfaces;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CoreZipCode.Services.ZipCode.ViaCepApi
{
    /// <summary>
    /// Provides an implementation of a zip code lookup service using the ViaCep API for Brazilian addresses.       
    /// </summary>
    /// <remarks>ViaCep enables retrieval of address information based on zip code, state, city, and street
    /// parameters. This class extends <see cref="ZipCodeBaseService"/> to construct ViaCep-specific request URLs. Use this service to
    /// integrate Brazilian postal code queries into your application. For more information about the ViaCep API, see
    /// https://viacep.com.br.</remarks>
    public class ViaCep : ZipCodeBaseService
    {
        private const string ZipCodeSizeErrorMessage = "Invalid ZipCode Size";
        private const string ZipCodeFormatErrorMessage = "Invalid ZipCode Format";

        public ViaCep() { }

        public ViaCep(HttpClient httpClient) : base(httpClient) { }

        public ViaCep(IApiHandler apiHandler) : base(apiHandler) { }

        public override string SetZipCodeUrl(string zipCode)
            => $"https://viacep.com.br/ws/{ValidateZipCode(zipCode)}/json/";

        public override string SetZipCodeUrlBy(string state, string city, string street)
            => $"https://viacep.com.br/ws/{ValidateParam("State", state, 2)}/{ValidateParam("City", city)}/{ValidateParam("Street", street)}/json/";

        private static string ValidateParam(string name, string value, int size = 3)
        {
            var trimmed = value.Trim();
            if (string.IsNullOrEmpty(trimmed) || trimmed.Length < size)
                throw new ViaCepException($"Invalid {name}, parameter below size of {size} characters.");
            return trimmed;
        }

        private static string ValidateZipCode(string zipCode)
        {
            var clean = zipCode.Trim().Replace("-", "");
            if (clean.Length != 8)
                throw new ViaCepException(ZipCodeSizeErrorMessage);
            if (!Regex.IsMatch(clean, @"^\d{8}$"))
                throw new ViaCepException(ZipCodeFormatErrorMessage);
            return clean;
        }
    }
}
