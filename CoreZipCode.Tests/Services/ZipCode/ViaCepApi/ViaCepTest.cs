using CoreZipCode.Services.ZipCode.ViaCepApi;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoreZipCode.Tests.Services.ZipCode.ViaCepApi
{
    public class ViaCepTest
    {
        private const string ExpectedResponse = "{\n  \"cep\": \"14810-100\",\n  \"logradouro\": \"Rua Barão do Rio Branco\",\n  \"complemento\": \"\",\n  \"bairro\": \"Vila Xavier (Vila Xavier)\",\n  \"localidade\": \"Araraquara\",\n  \"uf\": \"SP\",\n  \"ibge\": \"3503208\",\n  \"gia\": \"1818\",\n  \"ddd\": \"16\",\n  \"siafi\": \"7107\"\n}";
        private const string ExpectedListResponse = "[\n  {\n    \"cep\": \"14810-100\",\n    \"logradouro\": \"Rua Barão do Rio Branco\",\n    \"complemento\": \"\",\n    \"bairro\": \"Vila Xavier (Vila Xavier)\",\n    \"localidade\": \"Araraquara\",\n    \"uf\": \"SP\",\n    \"ibge\": \"3503208\",\n    \"gia\": \"1818\",\n    \"ddd\": \"16\",\n  \"siafi\": \"7107\"  }\n]";
        private const string ExpectedState = "SP";
        private const string ExpectedCity = "Araraquara";
        private const string ZipCodeTest = "14810-100";
        private const string SendAsync = "SendAsync";
        private const string MockUri = "https://unit.test.com/";
        private const string ViaCepParameterState = "sp";
        private const string ViaCepParameterCity = "araraquara";
        private const string ViaCepParameterStreet = "barão do rio";
        private const string InvalidStreetMessage = "Invalid Street, parameter below size of 3 characters.";
        private const string InvalidCityMessage = "Invalid City, parameter below size of 3 characters.";
        private const string InvalidStateMessage = "Invalid State, parameter below size of 2 characters.";
        private const string InvalidZipCodeFormatMessage = "Invalid ZipCode Format";
        private const string InvalidZipCodeSizeMessage = "Invalid ZipCode Size";

        private readonly ViaCep _service;
        private readonly ViaCep _serviceList;

        public ViaCepTest()
        {
            _service = ConfigureService(ExpectedResponse);
            _serviceList = ConfigureService(ExpectedListResponse);
        }

        private static ViaCep ConfigureService(string response)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    SendAsync,
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(response),
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(MockUri)
            };

            return new ViaCep(httpClient);
        }

        [Fact]
        public static void ConstructorTest()
        {
            var actual = new ViaCep();
            Assert.NotNull(actual);
        }

        [Fact]
        public void MustGetSingleZipCodeJsonString()
        {
            var actual = _service.Execute(ZipCodeTest);

            Assert.Equal(ExpectedResponse, actual);
        }

        [Fact]
        public void MustGetListZipCodeJsonString()
        {
            var actual = _serviceList.Execute(ViaCepParameterState, ViaCepParameterCity, ViaCepParameterStreet);

            Assert.Equal(ExpectedListResponse, actual);
        }

        [Fact]
        public void MustGetSingleZipCodeObject()
        {
            var actual = _service.GetAddress<ViaCepAddressModel>(ZipCodeTest);

            Assert.IsType<ViaCepAddressModel>(actual);
            Assert.Equal(ExpectedCity, actual.City);
            Assert.Equal(ExpectedState, actual.State);
        }

        [Fact]
        public void MustGetZipCodeObjectList()
        {
            var actual = _serviceList.ListAddresses<ViaCepAddressModel>(ViaCepParameterState, ViaCepParameterCity, ViaCepParameterStreet);

            Assert.IsType<List<ViaCepAddressModel>>(actual);
            Assert.True(actual.Count > 0);
            Assert.Equal(ExpectedCity, actual[0].City);
            Assert.Equal(ExpectedState, actual[0].State);
        }

        [Fact]
        public void MustThrowTheExceptions()
        {
            var exception = Assert.Throws<ViaCepException>(() => _service.Execute(" 12345-67 "));
            Assert.Equal(InvalidZipCodeSizeMessage, exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute(" 123A5-678 "));
            Assert.Equal(InvalidZipCodeFormatMessage, exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute("U", "Araraquara", "barão do rio"));
            Assert.Equal(InvalidStateMessage, exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute("SP", "Ar", "barão do rio"));
            Assert.Equal(InvalidCityMessage, exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute("SP", "Ara", "ba"));
            Assert.Equal(InvalidStreetMessage, exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute("", "Araraquara", "barão do rio"));
            Assert.Equal(InvalidStateMessage, exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute("SP", "", "barão do rio"));
            Assert.Equal(InvalidCityMessage, exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute("SP", "Ara", ""));
            Assert.Equal(InvalidStreetMessage, exception.Message);
        }

        [Fact]
        public async Task MustGetSingleZipCodeJsonStringAsync()
        {
            var actual = await _service.ExecuteAsync(ZipCodeTest);

            Assert.Equal(ExpectedResponse, actual);
        }

        [Fact]
        public async Task MustGetListZipCodeJsonStringAsync()
        {
            var actual = await _serviceList.ExecuteAsync(ViaCepParameterState, ViaCepParameterCity, ViaCepParameterStreet);

            Assert.Equal(ExpectedListResponse, actual);
        }

        [Fact]
        public async Task MustGetSingleZipCodeObjectAsync()
        {
            var actual = await _service.GetAddressAsync<ViaCepAddressModel>(ZipCodeTest);

            Assert.IsType<ViaCepAddressModel>(actual);
            Assert.Equal(ExpectedCity, actual.City);
            Assert.Equal(ExpectedState, actual.State);
        }

        [Fact]
        public async Task MustGetZipCodeObjectListAsync()
        {
            var actual = await _serviceList.ListAddressesAsync<ViaCepAddressModel>(ViaCepParameterState, ViaCepParameterCity, ViaCepParameterStreet);

            Assert.IsType<List<ViaCepAddressModel>>(actual);
            Assert.True(actual.Count > 0);
            Assert.Equal(ExpectedCity, actual[0].City);
            Assert.Equal(ExpectedState, actual[0].State);
        }

        [Fact]
        public async Task MustThrowTheExceptionsAsync()
        {
            var exception = await Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync(" 12345-67 "));
            Assert.Equal(InvalidZipCodeSizeMessage, exception.Message);

            exception = await Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync(" 123A5-678 "));
            Assert.Equal(InvalidZipCodeFormatMessage, exception.Message);

            exception = await Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("U", "Araraquara", "barão do rio"));
            Assert.Equal(InvalidStateMessage, exception.Message);

            exception = await Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "Ar", "barão do rio"));
            Assert.Equal(InvalidCityMessage, exception.Message);

            exception = await Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "Ara", "ba"));
            Assert.Equal(InvalidStreetMessage, exception.Message);

            exception = await Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("", "Araraquara", "barão do rio"));
            Assert.Equal(InvalidStateMessage, exception.Message);

            exception = await Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "", "barão do rio"));
            Assert.Equal(InvalidCityMessage, exception.Message);

            exception = await Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "Ara", ""));
            Assert.Equal(InvalidStreetMessage, exception.Message);
        }
    }
}