using CoreZipCode.Interfaces;
using CoreZipCode.Result;
using CoreZipCode.Services.ZipCode.ViaCepApi;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CoreZipCode.Tests.Services.ZipCode.ViaCepApi
{
    public class ViaCepTest
    {
        private const string ValidCep = "14810100";
        private const string SingleAddressJson = "{\n  \"cep\": \"14810-100\",\n  \"logradouro\": \"Rua Barão do Rio Branco\",\n  \"complemento\": \"\",\n  \"bairro\": \"Vila Xavier (Vila Xavier)\",\n  \"localidade\": \"Araraquara\",\n  \"uf\": \"SP\",\n  \"ibge\": \"3503208\",\n  \"gia\": \"1818\",\n  \"ddd\": \"16\",\n  \"siafi\": \"7107\"\n}";
        private const string ListAddressJson = "[\n  {\n    \"cep\": \"14810-100\",\n    \"logradouro\": \"Rua Barão do Rio Branco\",\n    \"complemento\": \"\",\n    \"bairro\": \"Vila Xavier (Vila Xavier)\",\n    \"localidade\": \"Araraquara\",\n    \"uf\": \"SP\",\n    \"ibge\": \"3503208\",\n    \"gia\": \"1818\",\n    \"ddd\": \"16\",\n  \"siafi\": \"7107\"  }\n]";

        private readonly Mock<IApiHandler> _handlerMock = new();
        private readonly ViaCep _service;

        public ViaCepTest()
        {
            _service = new ViaCep(_handlerMock.Object);
        }

        [Fact]
        public void Constructor_Creates_Instance()
        {
            new ViaCep();
        }

        [Fact]
        public void Constructor_Creates_Instance_With_HttpClient()
        {
            new ViaCep(new HttpClient());
        }

        [Fact]
        public void Constructor_With_Null_ApiHandler_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ViaCep((IApiHandler)null));
        }

        [Fact]
        public async Task GetAddressAsync_ValidCep_Returns_Success()
        {
            _handlerMock
                .Setup(x => x.CallApiAsync($"https://viacep.com.br/ws/{ValidCep}/json/"))
                .ReturnsAsync(Result<string>.Success(SingleAddressJson));

            var result = await _service.GetAddressAsync<ViaCepAddressModel>(ValidCep);

            Assert.True(result.IsSuccess);
            Assert.Equal("Araraquara", result.Value.City);
            Assert.Equal("SP", result.Value.State);
        }

        [Fact]
        public async Task GetAddressAsync_ApiReturns404_Returns_Failure()
        {
            _handlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Failure(new ApiError(HttpStatusCode.NotFound, "Not found")));

            var result = await _service.GetAddressAsync<ViaCepAddressModel>("00000000");

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.NotFound, result.Error.StatusCode);
        }

        [Fact]
        public async Task GetAddressAsync_ApiReturnsNullBody_Returns_NotFound()
        {
            _handlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Success("null"));

            var result = await _service.GetAddressAsync<ViaCepAddressModel>(ValidCep);

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.NotFound, result.Error.StatusCode);
            Assert.Equal("Address not found or empty response.", result.Error.Message);
        }

        [Fact]
        public async Task GetAddressAsync_ApiReturnsInvalidJson_Returns_UnprocessableEntity()
        {
            var invalidJson = "{ invalid json }";
            _handlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Success(invalidJson));

            var result = await _service.GetAddressAsync<ViaCepAddressModel>(ValidCep);

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, result.Error.StatusCode);
            Assert.Equal("Failed to parse API response.", result.Error.Message);
            Assert.NotNull(result.Error.Detail);
            Assert.Equal(invalidJson, result.Error.ResponseBody);
        }

        [Fact]
        public async Task GetAddressAsync_InvalidCep_Throws_ViaCepException()
        {
            var ex = await Assert.ThrowsAsync<ViaCepException>(
                () => _service.GetAddressAsync<ViaCepAddressModel>("123"));

            Assert.Contains("Invalid ZipCode Size", ex.Message);
        }

        [Fact]
        public async Task GetAddressAsync_NonNumericCep_Throws_ViaCepException()
        {
            var ex = await Assert.ThrowsAsync<ViaCepException>(
                () => _service.GetAddressAsync<ViaCepAddressModel>("ABCDEF12"));

            Assert.Contains("Invalid ZipCode Format", ex.Message);
        }

        [Fact]
        public async Task ListAddressesAsync_ValidParams_Returns_Success_List()
        {
            _handlerMock
                .Setup(x => x.CallApiAsync("https://viacep.com.br/ws/SP/Araraquara/barão do rio/json/"))
                .ReturnsAsync(Result<string>.Success(ListAddressJson));

            var result = await _service.ListAddressesAsync<ViaCepAddressModel>("SP", "Araraquara", "barão do rio");

            Assert.True(result.IsSuccess);
            Assert.Single(result.Value);
            Assert.Equal("Araraquara", result.Value[0].City);
            Assert.Equal("SP", result.Value[0].State);
        }

        [Fact]
        public async Task ListAddressesAsync_ApiReturnsEmptyList_Returns_Success_EmptyList()
        {
            _handlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Success("[]"));

            var result = await _service.ListAddressesAsync<ViaCepAddressModel>("SP", "City", "Street");

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task ListAddressesAsync_ApiReturnsNull_Returns_Success_EmptyList()
        {
            _handlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Success("null"));

            var result = await _service.ListAddressesAsync<ViaCepAddressModel>("SP", "City", "Street");

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task ListAddressesAsync_ApiReturnsInvalidJson_Returns_UnprocessableEntity()
        {
            var invalidJson = "[ invalid json ]";
            _handlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Success(invalidJson));

            var result = await _service.ListAddressesAsync<ViaCepAddressModel>("SP", "City", "Street");

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, result.Error.StatusCode);
            Assert.Equal("Failed to parse address list.", result.Error.Message);
            Assert.NotNull(result.Error.Detail);
            Assert.Equal(invalidJson, result.Error.ResponseBody);
        }

        [Fact]
        public async Task ListAddressesAsync_ApiReturnsFailure_Returns_Failure()
        {
            var error = new ApiError(HttpStatusCode.ServiceUnavailable, "Service unavailable");
            _handlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Failure(error));

            var result = await _service.ListAddressesAsync<ViaCepAddressModel>("SP", "City", "Street");

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, result.Error.StatusCode);
            Assert.Equal("Service unavailable", result.Error.Message);
        }

        [Fact]
        public async Task ListAddressesAsync_InvalidState_Throws_ViaCepException()
        {
            var ex = await Assert.ThrowsAsync<ViaCepException>(
                () => _service.ListAddressesAsync<ViaCepAddressModel>("X", "City", "Street"));

            Assert.Contains("Invalid State", ex.Message);
        }

        [Fact]
        public async Task ListAddressesAsync_EmptyState_Throws_ViaCepException()
        {
            var ex = await Assert.ThrowsAsync<ViaCepException>(
                () => _service.ListAddressesAsync<ViaCepAddressModel>("", "City", "Street"));

            Assert.Contains("Invalid State", ex.Message);
        }

        [Fact]
        public async Task ListAddressesAsync_InvalidCity_Throws_ViaCepException()
        {
            var ex = await Assert.ThrowsAsync<ViaCepException>(
                () => _service.ListAddressesAsync<ViaCepAddressModel>("SP", "AB", "Street"));

            Assert.Contains("Invalid City", ex.Message);
        }

        [Fact]
        public async Task ListAddressesAsync_EmptyCity_Throws_ViaCepException()
        {
            var ex = await Assert.ThrowsAsync<ViaCepException>(
                () => _service.ListAddressesAsync<ViaCepAddressModel>("SP", "", "Street"));

            Assert.Contains("Invalid City", ex.Message);
        }

        [Fact]
        public async Task ListAddressesAsync_InvalidStreet_Throws_ViaCepException()
        {
            var ex = await Assert.ThrowsAsync<ViaCepException>(
                () => _service.ListAddressesAsync<ViaCepAddressModel>("SP", "City", "AB"));

            Assert.Contains("Invalid Street", ex.Message);
        }

        [Fact]
        public async Task ListAddressesAsync_EmptyStreet_Throws_ViaCepException()
        {
            var ex = await Assert.ThrowsAsync<ViaCepException>(
                () => _service.ListAddressesAsync<ViaCepAddressModel>("SP", "City", ""));

            Assert.Contains("Invalid Street", ex.Message);
        }

        [Fact]
        [Obsolete("Will be removed in next version")]
        public async Task ExecuteAsync_ValidCep_Returns_Json_String()
        {
            _handlerMock
                .Setup(x => x.CallApiAsync($"https://viacep.com.br/ws/{ValidCep}/json/"))
                .ReturnsAsync(Result<string>.Success(SingleAddressJson));

            var result = await _service.ExecuteAsync(ValidCep);

            Assert.Equal(SingleAddressJson, result);
        }

        [Fact]
        [Obsolete("Will be removed in next version")]
        public async Task ExecuteAsync_ApiReturnsFailure_Throws_HttpRequestException()
        {
            var error = new ApiError(HttpStatusCode.NotFound, "Not found");
            _handlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Failure(error));

            var ex = await Assert.ThrowsAsync<HttpRequestException>(
                () => _service.ExecuteAsync(ValidCep));

            Assert.Equal("Not found", ex.Message);
        }

        [Fact]
        [Obsolete("Will be removed in next version")]
        public void Execute_Always_Throws_NotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => _service.Execute(ValidCep));
        }

        [Fact]
        [Obsolete("Will be removed in next version")]
        public void GetAddress_Always_Throws_NotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => _service.GetAddress<ViaCepAddressModel>(ValidCep));
        }

        [Fact]
        public void SetZipCodeUrl_Generates_Correct_Url()
        {
            var url = _service.SetZipCodeUrl("01001000");
            Assert.Equal("https://viacep.com.br/ws/01001000/json/", url);
        }

        [Fact]
        public void SetZipCodeUrl_With_Hyphen_Generates_Correct_Url()
        {
            var url = _service.SetZipCodeUrl("01001-000");
            Assert.Equal("https://viacep.com.br/ws/01001000/json/", url);
        }

        [Fact]
        public void SetZipCodeUrl_With_Spaces_Generates_Correct_Url()
        {
            var url = _service.SetZipCodeUrl(" 01001000 ");
            Assert.Equal("https://viacep.com.br/ws/01001000/json/", url);
        }

        [Fact]
        public void SetZipCodeUrlBy_Generates_Correct_Url()
        {
            var url = _service.SetZipCodeUrlBy("rj", "rio de janeiro", "praia de botafogo");
            Assert.Equal("https://viacep.com.br/ws/rj/rio de janeiro/praia de botafogo/json/", url);
        }

        [Fact]
        public void SetZipCodeUrlBy_With_Spaces_Generates_Correct_Url()
        {
            var url = _service.SetZipCodeUrlBy(" SP ", " Araraquara ", " Street Name ");
            Assert.Equal("https://viacep.com.br/ws/SP/Araraquara/Street Name/json/", url);
        }
    }
}
