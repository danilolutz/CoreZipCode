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
        private const string ExpectedResponse = "{\n  \"cep\": \"14810-100\",\n  \"logradouro\": \"Rua Barão do Rio Branco\",\n  \"complemento\": \"\",\n  \"bairro\": \"Vila Xavier (Vila Xavier)\",\n  \"localidade\": \"Araraquara\",\n  \"uf\": \"SP\",\n  \"unidade\": \"\",\n  \"ibge\": \"3503208\",\n  \"gia\": \"1818\"\n}";
        private const string ExpectedListResponse = "[\n  {\n    \"cep\": \"14810-100\",\n    \"logradouro\": \"Rua Barão do Rio Branco\",\n    \"complemento\": \"\",\n    \"bairro\": \"Vila Xavier (Vila Xavier)\",\n    \"localidade\": \"Araraquara\",\n    \"uf\": \"SP\",\n    \"unidade\": \"\",\n    \"ibge\": \"3503208\",\n    \"gia\": \"1818\"\n  }\n]";
        private const string ExpectedState = "SP";
        private const string ExpectedCity = "Araraquara";
        private const string ZipCodeTest = "14810-100";
        private const string SendAsync = "SendAsync";
        private const string MockUri = "https://unit.test.com/";
        private const string ViaCepParameterState = "sp";
        private const string ViaCepParameterCity = "araraquara";
        private const string ViaCepParameterStreet = "barão do rio";
        private const string InvalidStreetMessage = "Invalid Street Param";
        private const string InvalidCityMessage = "Invalid City Param";
        private const string InvalidStateMessage = "Invalid State Param";
        private const string InvalidZipCodeFormatMessage = "Invalid ZipCode Format";
        private const string InvalidZipCodeSizeMessage = "Invalid ZipCode Size";

        private readonly IList<ViaCepAddress> _expectedObjectListResponse = new List<ViaCepAddress>();
        private readonly ViaCep _service;
        private readonly ViaCep _serviceList;
        private Mock<HttpMessageHandler> _handlerMock;

        public ViaCepTest()
        {
            var expectedObjectResponse = new ViaCepAddress
            {
                ZipCode = "14810-100",
                Address1 = "Rua Barão do Rio Branco",
                Complement = string.Empty,
                Neighborhood = "Vila Xavier (Vila Xavier)",
                City = "Araraquara",
                State = "SP",
                Unity = string.Empty,
                IBGE = "3503208",
                GIA = "1818"
            };

            _expectedObjectListResponse.Add(expectedObjectResponse);

            _service = ConfigureService(ExpectedResponse);
            _serviceList = ConfigureService(ExpectedListResponse);
        }

        private ViaCep ConfigureService(string response)
        {
            _handlerMock = new Mock<HttpMessageHandler>();

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    SendAsync,
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(response),
                })
                .Verifiable();

            var httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri(MockUri)
            };

            return new ViaCep(httpClient);
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
            var actual = _service.GetAddress<ViaCepAddress>(ZipCodeTest);

            Assert.IsType<ViaCepAddress>(actual);
            Assert.Equal(ExpectedCity, actual.City);
            Assert.Equal(ExpectedState, actual.State);
        }

        [Fact]
        public void MustGetZipCodeObjectList()
        {
            var actual = _serviceList.ListAddresses<ViaCepAddress>(ViaCepParameterState, ViaCepParameterCity, ViaCepParameterStreet);

            Assert.IsType<List<ViaCepAddress>>(actual);
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
        public async void MustGetSingleZipCodeJsonStringAsync()
        {
            var actual = await _service.ExecuteAsync(ZipCodeTest);

            Assert.Equal(ExpectedResponse, actual);
        }

        [Fact]
        public async void MustGetListZipCodeJsonStringAsync()
        {
            var actual = await _serviceList.ExecuteAsync(ViaCepParameterState, ViaCepParameterCity, ViaCepParameterStreet);

            Assert.Equal(ExpectedListResponse, actual);
        }

        [Fact]
        public async void MustGetSingleZipCodeObjectAsync()
        {
            var actual = await _service.GetAddressAsync<ViaCepAddress>(ZipCodeTest);

            Assert.IsType<ViaCepAddress>(actual);
            Assert.Equal(ExpectedCity, actual.City);
            Assert.Equal(ExpectedState, actual.State);
        }

        [Fact]
        public async void MustGetZipCodeObjectListAsync()
        {
            var actual = await _serviceList.ListAddressesAsync<ViaCepAddress>(ViaCepParameterState, ViaCepParameterCity, ViaCepParameterStreet);

            Assert.IsType<List<ViaCepAddress>>(actual);
            Assert.True(actual.Count > 0);
            Assert.Equal(ExpectedCity, actual[0].City);
            Assert.Equal(ExpectedState, actual[0].State);
        }

        [Fact]
        public void MustThrowTheExceptionsAsync()
        {
            var exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync(" 12345-67 "));
            Assert.Equal(InvalidZipCodeSizeMessage, exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync(" 123A5-678 "));
            Assert.Equal(InvalidZipCodeFormatMessage, exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("U", "Araraquara", "barão do rio"));
            Assert.Equal(InvalidStateMessage, exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "Ar", "barão do rio"));
            Assert.Equal(InvalidCityMessage, exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "Ara", "ba"));
            Assert.Equal(InvalidStreetMessage, exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("", "Araraquara", "barão do rio"));
            Assert.Equal(InvalidStateMessage, exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "", "barão do rio"));
            Assert.Equal(InvalidCityMessage, exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "Ara", ""));
            Assert.Equal(InvalidStreetMessage, exception.Result.Message);
        }
    }
}