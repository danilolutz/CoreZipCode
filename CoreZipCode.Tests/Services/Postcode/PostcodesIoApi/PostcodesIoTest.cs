using CoreZipCode.Interfaces;
using CoreZipCode.Result;
using CoreZipCode.Services.Postcode.PostcodesIoApi;
using CoreZipCode.Services.ZipCode.SmartyApi;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CoreZipCode.Tests.Services.Postcode.PostcodesIoApi
{
    public class PostcodesIoTests
    {
        private const string ValidPostcode = "OX49 5NU";

        private const string SuccessJson = """{"status":200,"result":[{"postcode":"OX49 5NU","quality":1,"eastings":464447,"northings":195647,"country":"England","nhs_ha":"South Central","longitude":-1.069752,"latitude":51.655929,"european_electoral_region":"South East","primary_care_trust":"Oxfordshire","region":"South East","lsoa":"South Oxfordshire 011B","msoa":"South Oxfordshire 011","incode":"5NU","outcode":"OX49","parliamentary_constituency":"Henley","admin_district":"South Oxfordshire","parish":"Brightwell Baldwin","admin_county":"Oxfordshire","admin_ward":"Chalgrove","ced":"Chalgrove and Watlington","ccg":"NHS Oxfordshire","nuts":"Oxfordshire","codes":{"admin_district":"E07000179","admin_county":"E10000025","admin_ward":"E05009735","parish":"E04008109","parliamentary_constituency":"E14000742","ccg":"E38000136","ced":"E58001238","nuts":"UKJ14"}}]}""";

        private readonly Mock<IApiHandler> _apiHandlerMock;
        private readonly PostcodesIo _service;

        public PostcodesIoTests()
        {
            _apiHandlerMock = new Mock<IApiHandler>();
            _service = new PostcodesIo(_apiHandlerMock.Object);
        }

        [Fact]
        public void Constructor_Should_Create_Instance()
        {
            new PostcodesIo();
        }

        [Fact]
        public void Constructor_Should_Create_Instance_With_HttpClient()
        {
            new PostcodesIo(new HttpClient());
        }

        [Fact]
        public async Task GetPostcodeAsync_ValidPostcode_Returns_Success_Result()
        {
            _apiHandlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Success(SuccessJson));

            var result = await _service.GetPostcodeAsync<PostcodesIoModel>(ValidPostcode);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(200, result.Value.Status);
            Assert.Equal("England", result.Value.Result[0].Country);
            Assert.Equal(1, result.Value.Result[0].Quality);
        }

        [Fact]
        public async Task GetPostcodeAsync_ApiReturnsNotFound_Returns_Failure()
        {
            var error = new ApiError(HttpStatusCode.NotFound, "Not found");
            _apiHandlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Failure(error));

            var result = await _service.GetPostcodeAsync<PostcodesIoModel>("INVALID");

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.NotFound, result.Error.StatusCode);
        }

        [Fact]
        public async Task GetPostcodeAsync_ApiReturnsMalformedJson_Returns_UnprocessableEntity()
        {
            _apiHandlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Success("{ invalid json }"));

            var result = await _service.GetPostcodeAsync<PostcodesIoModel>(ValidPostcode);

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, result.Error.StatusCode);
        }

        [Fact]
        public void SetPostcodeUrl_Should_Generate_Correct_Url()
        {
            var service = new PostcodesIo();
            var url = service.SetPostcodeUrl("SW1A 1AA");

            Assert.Equal("https://api.postcodes.io/postcodes?q=SW1A%201AA", url);
        }
    }
}
