using Xunit;
using CoreZipCode.Interfaces;
using CoreZipCode.Services;

namespace CoreZipCode.Tests.Services
{
    public class ViaCepTest
    {
        private readonly ZipCodeBaseService _service;

        public ViaCepTest()
        {
            _service = new ViaCep();
        }

        [Fact]
        public void MustGetSingleZipCodeJsonString()
        {
            string expected = "{\n  \"cep\": \"14810-100\",\n  \"logradouro\": \"Rua Barão do Rio Branco\",\n  \"complemento\": \"\",\n  \"bairro\": \"Vila Xavier (Vila Xavier)\",\n  \"localidade\": \"Araraquara\",\n  \"uf\": \"SP\",\n  \"unidade\": \"\",\n  \"ibge\": \"3503208\",\n  \"gia\": \"1818\"\n}";
            var actual = _service.Execute("14810-100");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MustGetListZipCodeJsonString()
        {
            string expected = "[\n  {\n    \"cep\": \"14810-100\",\n    \"logradouro\": \"Rua Barão do Rio Branco\",\n    \"complemento\": \"\",\n    \"bairro\": \"Vila Xavier (Vila Xavier)\",\n    \"localidade\": \"Araraquara\",\n    \"uf\": \"SP\",\n    \"unidade\": \"\",\n    \"ibge\": \"3503208\",\n    \"gia\": \"1818\"\n  }\n]";
            var actual = _service.Execute("sp", "araraquara", "barão do rio");

            Assert.Equal(expected, actual);
        }
    }
}
