using CoreZipCode.Services.ViaCepApi;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoreZipCode.Tests.Services.ViaCepApi
{
    public class ViaCepTest
    {
        private readonly string _expectedResponse = "{\n  \"cep\": \"14810-100\",\n  \"logradouro\": \"Rua Barão do Rio Branco\",\n  \"complemento\": \"\",\n  \"bairro\": \"Vila Xavier (Vila Xavier)\",\n  \"localidade\": \"Araraquara\",\n  \"uf\": \"SP\",\n  \"unidade\": \"\",\n  \"ibge\": \"3503208\",\n  \"gia\": \"1818\"\n}";
        private readonly string _expectedListResponse = "[\n  {\n    \"cep\": \"14810-100\",\n    \"logradouro\": \"Rua Barão do Rio Branco\",\n    \"complemento\": \"\",\n    \"bairro\": \"Vila Xavier (Vila Xavier)\",\n    \"localidade\": \"Araraquara\",\n    \"uf\": \"SP\",\n    \"unidade\": \"\",\n    \"ibge\": \"3503208\",\n    \"gia\": \"1818\"\n  }\n]";
        private readonly ViaCepAddress _expectedObjectResponse;
        private readonly IList<ViaCepAddress> _expectedObjectListResponse = new List<ViaCepAddress>();
        private ViaCep _service;
        private ViaCep _serviceList;
        private Mock<HttpMessageHandler> _handlerMock;

        public ViaCepTest()
        {
            _expectedObjectResponse = new ViaCepAddress
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

            _expectedObjectListResponse.Add(_expectedObjectResponse);

            _service = ConfigureService(_expectedResponse);
            _serviceList = ConfigureService(_expectedListResponse);
        }

        private ViaCep ConfigureService(string response)
        {
            _handlerMock = new Mock<HttpMessageHandler>();

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
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
                BaseAddress = new Uri("https://unit.test.com/"),
            };

            return new ViaCep(httpClient);
        }

        [Fact]
        public void MustGetSingleZipCodeJsonString()
        {
            var actual = _service.Execute("14810-100");

            Assert.Equal(_expectedResponse, actual);
        }

        [Fact]
        public void MustGetListZipCodeJsonString()
        {
            var actual = _serviceList.Execute("sp", "araraquara", "barão do rio");

            Assert.Equal(_expectedListResponse, actual);
        }

        [Fact]
        public void MustGetSingleZipCodeObject()
        {
            var actual = _service.GetAddress<ViaCepAddress>("14810-100");

            Assert.IsType<ViaCepAddress>(actual);
            Assert.Equal("Araraquara", actual.City);
            Assert.Equal("SP", actual.State);
        }

        [Fact]
        public void MustGetZipCodeObjectList()
        {
            var actual = _serviceList.ListAddresses<ViaCepAddress>("sp", "araraquara", "barão do rio");

            Assert.IsType<List<ViaCepAddress>>(actual);
            Assert.True(actual.Count > 0);
            Assert.Equal("Araraquara", actual[0].City);
            Assert.Equal("SP", actual[0].State);
        }

        [Fact]
        public void MustThrowTheExceptions()
        {
            var exception = Assert.Throws<ViaCepException>(() => _service.Execute(" 12345-67 "));
            Assert.Equal("Invalid ZipCode Size", exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute(" 123A5-678 "));
            Assert.Equal("Invalid ZipCode Format", exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute("U", "Araraquara", "barão do rio"));
            Assert.Equal("Invalid State Param", exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute("SP", "Ar", "barão do rio"));
            Assert.Equal("Invalid City Param", exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute("SP", "Ara", "ba"));
            Assert.Equal("Invalid Street Param", exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute("", "Araraquara", "barão do rio"));
            Assert.Equal("Invalid State Param", exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute("SP", "", "barão do rio"));
            Assert.Equal("Invalid City Param", exception.Message);

            exception = Assert.Throws<ViaCepException>(() => _service.Execute("SP", "Ara", ""));
            Assert.Equal("Invalid Street Param", exception.Message);
        }

        [Fact]
        public async void MustGetSingleZipCodeJsonStringAsync()
        {
            var actual = await _service.ExecuteAsync("14810-100");

            Assert.Equal(_expectedResponse, actual);
        }

        [Fact]
        public async void MustGetListZipCodeJsonStringAsync()
        {
            var actual = await _serviceList.ExecuteAsync("sp", "araraquara", "barão do rio");

            Assert.Equal(_expectedListResponse, actual);
        }

        [Fact]
        public async void MustGetSingleZipCodeObjectAsync()
        {
            var actual = await _service.GetAddressAsync<ViaCepAddress>("14810-100");

            Assert.IsType<ViaCepAddress>(actual);
            Assert.Equal("Araraquara", actual.City);
            Assert.Equal("SP", actual.State);
        }

        [Fact]
        public async void MustGetZipCodeObjectListAsync()
        {
            var actual = await _serviceList.ListAddressesAsync<ViaCepAddress>("sp", "araraquara", "barão do rio");

            Assert.IsType<List<ViaCepAddress>>(actual);
            Assert.True(actual.Count > 0);
            Assert.Equal("Araraquara", actual[0].City);
            Assert.Equal("SP", actual[0].State);
        }

        [Fact]
        public void MustThrowTheExceptionsAsync()
        {
            var exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync(" 12345-67 "));
            Assert.Equal("Invalid ZipCode Size", exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync(" 123A5-678 "));
            Assert.Equal("Invalid ZipCode Format", exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("U", "Araraquara", "barão do rio"));
            Assert.Equal("Invalid State Param", exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "Ar", "barão do rio"));
            Assert.Equal("Invalid City Param", exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "Ara", "ba"));
            Assert.Equal("Invalid Street Param", exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("", "Araraquara", "barão do rio"));
            Assert.Equal("Invalid State Param", exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "", "barão do rio"));
            Assert.Equal("Invalid City Param", exception.Result.Message);

            exception = Assert.ThrowsAsync<ViaCepException>(() => _service.ExecuteAsync("SP", "Ara", ""));
            Assert.Equal("Invalid Street Param", exception.Result.Message);
        }
    }
}