using CoreZipCode.Interfaces;
using CoreZipCode.Result;
using CoreZipCode.Services.Postcode.PostalpincodeInApi;
using CoreZipCode.Services.ZipCode.ViaCepApi;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CoreZipCode.Tests.Services.Postcode.PostalpincodeInApi
{
    public class PostalpincodeInTest
    {
        private const string ValidPincode = "110001";
        private const string ExpectedJsonResponse = "{\"Message\":\"Number of Post office(s) found: 21\",\"Status\":\"Success\",\"PostOffice\":[{\"Name\":\"Baroda House\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Bengali Market\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Bhagat Singh Market\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Connaught Place\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Constitution House\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Election Commission\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Janpath\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Krishi Bhawan\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Lady Harding Medical College\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"New Delhi \",\"Description\":\"\",\"BranchType\":\"Head Post Office\",\"DeliveryStatus\":\"Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"New Delhi\",\"Division\":\"New Delhi GPO\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"North Avenue\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Parliament House\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Patiala House\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Pragati Maidan\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Pragati Maidan Camp\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Rail Bhawan\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Sansad Marg\",\"Description\":\"\",\"BranchType\":\"Head Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Sansadiya Soudh\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Secretariat North\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Shastri Bhawan\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Supreme Court\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"}]}";

        private readonly PostalpincodeIn _service;
        private readonly Mock<IApiHandler> _apiHandlerMock;

        public PostalpincodeInTest()
        {
            _apiHandlerMock = new Mock<IApiHandler>();
            _service = new PostalpincodeIn(_apiHandlerMock.Object);
        }

        [Fact]
        public void Constructor_Should_Create_Instance_Without_Parameters()
        {
            var service = new PostalpincodeIn();
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_With_Null_ApiHandler_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PostalpincodeIn((IApiHandler)null));
        }

        [Fact]
        public void Constructor_Should_Create_Instance_With_HttpClient()
        {
            new PostalpincodeIn(new HttpClient());
        }

        [Fact]
        public async Task GetPostcodeAsync_With_Valid_Pincode_Should_Return_Success_Result()
        {
            _apiHandlerMock
                .Setup(x => x.CallApiAsync($"http://postalpincode.in/api/pincode/{ValidPincode}"))
                .ReturnsAsync(Result<string>.Success(ExpectedJsonResponse));

            var result = await _service.GetPostcodeAsync<PostalpincodeInModel>(ValidPincode);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("Success", result.Value.Status);
            Assert.Equal("Baroda House", result.Value.PostOffice[0].Name);
            Assert.Equal("India", result.Value.PostOffice[0].Country);
            Assert.Equal(21, result.Value.PostOffice.Count);
        }

        [Fact]
        public async Task GetPostcodeAsync_With_Invalid_Pincode_Should_Return_Failure_From_ApiHandler()
        {
            var error = new ApiError(HttpStatusCode.NotFound, "Not Found", responseBody: "No records found");
            _apiHandlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Failure(error));

            var result = await _service.GetPostcodeAsync<PostalpincodeInModel>("000000");

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.NotFound, result.Error.StatusCode);
            Assert.Contains("Not Found", result.Error.Message);
        }

        [Fact]
        public async Task GetPostcodeAsync_With_Invalid_Pincode_Should_Return_Failure_From_EmptyResponse()
        {
            _apiHandlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Success(""));

            var result = await _service.GetPostcodeAsync<PostalpincodeInModel>("000000");

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.NotFound, result.Error.StatusCode);
            Assert.Contains("Postcode not found or empty response.", result.Error.Message);
        }

        [Fact]
        public async Task GetPostcodeAsync_With_Network_Error_Should_Propagate_Failure()
        {
            var networkError = new ApiError(HttpStatusCode.ServiceUnavailable, "Network error");
            _apiHandlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Failure(networkError));

            var result = await _service.GetPostcodeAsync<PostalpincodeInModel>(ValidPincode);

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, result.Error.StatusCode);
        }

        [Fact]
        public async Task GetPostcodeAsync_With_Malformed_Json_Should_Return_Failure_With_UnprocessableEntity()
        {
            _apiHandlerMock
                .Setup(x => x.CallApiAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<string>.Success("{{{ invalid json }}}"));

            var result = await _service.GetPostcodeAsync<PostalpincodeInModel>(ValidPincode);

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, result.Error.StatusCode);
            Assert.Contains("Failed to parse", result.Error.Message);
        }

        [Fact]
        public void SetPostcodeUrl_Should_Generate_Correct_Url()
        {
            var service = new PostalpincodeIn();

            var url = service.SetPostcodeUrl("400001");

            Assert.Equal("http://postalpincode.in/api/pincode/400001", url);
        }

        [Fact]
        [Obsolete("Will be removed in next version")]
        public void Execute_Always_Throws_NotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => _service.Execute("400001"));
        }

        [Fact]
        [Obsolete("Will be removed in next version")]
        public void GetAddress_Always_Throws_NotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => _service.GetPostcode<PostalpincodeInModel>("400001"));
        }
    }
}
