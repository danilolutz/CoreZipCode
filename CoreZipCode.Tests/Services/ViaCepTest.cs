using CoreZipCode.Interfaces;
using CoreZipCode.Services;
using System.Net.Http;
using Xunit;

namespace CoreZipCode.Tests.Services
{
    public class ViaCepTest
    {
        private readonly ZipCodeBaseService _service;
        private const string ViaCepParameterState = "sp";
        private const string ViaCepParameterCity = "araraquara";
        private const string ViaCepParameterStreet = "barão do rio";
        private const string PostalPinCodeTest = "14810-100";

        public ViaCepTest()
        {
            _service = new ViaCep(new HttpClient());
        }

        [Fact]
        public void MustGetSingleZipCodeJsonString()
        {
            string expected = "{\n  \"cep\": \"14810-100\",\n  \"logradouro\": \"Rua Barão do Rio Branco\",\n  \"complemento\": \"\",\n  \"bairro\": \"Vila Xavier (Vila Xavier)\",\n  \"localidade\": \"Araraquara\",\n  \"uf\": \"SP\",\n  \"unidade\": \"\",\n  \"ibge\": \"3503208\",\n  \"gia\": \"1818\"\n}";
            var actual = _service.Execute(PostalPinCodeTest);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MustGetListZipCodeJsonString()
        {
            string expected = "[\n  {\n    \"cep\": \"14810-100\",\n    \"logradouro\": \"Rua Barão do Rio Branco\",\n    \"complemento\": \"\",\n    \"bairro\": \"Vila Xavier (Vila Xavier)\",\n    \"localidade\": \"Araraquara\",\n    \"uf\": \"SP\",\n    \"unidade\": \"\",\n    \"ibge\": \"3503208\",\n    \"gia\": \"1818\"\n  }\n]";
            var actual = _service.Execute(ViaCepParameterState, ViaCepParameterCity, ViaCepParameterStreet);

            Assert.Equal(expected, actual);
        }
    }
}
