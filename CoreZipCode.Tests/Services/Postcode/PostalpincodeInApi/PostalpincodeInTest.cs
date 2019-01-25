using CoreZipCode.Services.Postcode.PostalpincodeInApi;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoreZipCode.Tests.Services.Postcode.PostalpincodeInApi
{
    public class PostalpincodeInTest
    {
        private readonly string _expectedResponse = "{\"Message\":\"Number of Post office(s) found: 21\",\"Status\":\"Success\",\"PostOffice\":[{\"Name\":\"Baroda House\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Bengali Market\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Bhagat Singh Market\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Connaught Place\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Constitution House\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Election Commission\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Janpath\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Krishi Bhawan\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Lady Harding Medical College\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"New Delhi \",\"Description\":\"\",\"BranchType\":\"Head Post Office\",\"DeliveryStatus\":\"Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"New Delhi\",\"Division\":\"New Delhi GPO\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"North Avenue\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Parliament House\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Patiala House\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Pragati Maidan\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Pragati Maidan Camp\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Rail Bhawan\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Sansad Marg\",\"Description\":\"\",\"BranchType\":\"Head Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Sansadiya Soudh\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Secretariat North\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Shastri Bhawan\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Supreme Court\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"}]}";
        private Mock<HttpMessageHandler> _handlerMock;
        private PostalpincodeIn _service;

        public PostalpincodeInTest()
        {
            _service = ConfigureService(_expectedResponse);
        }

        private PostalpincodeIn ConfigureService(string response)
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

            return new PostalpincodeIn(httpClient);
        }

        [Fact]
        public void MustGetPostCodes()
        {
            var actual = _service.Execute("110001");

            Assert.Equal(_expectedResponse, actual);
        }

        [Fact]
        public void MustGetPostCodesObject()
        {
            var actual = _service.GetPostcode<PostalpincodeInModel>("110001");

            Assert.IsType<PostalpincodeInModel>(actual);
            Assert.Equal("Success", actual.Status);
            Assert.Equal("Baroda House", actual.PostOffice[0].Name);
            Assert.Equal("India", actual.PostOffice[0].Country);
        }

        [Fact]
        public async void MustGetPostCodesAsync()
        {
            var actual = await _service.ExecuteAsync("110001");

            Assert.Equal(_expectedResponse, actual);
        }

        [Fact]
        public async void MustGetPostCodesObjectAsync()
        {
            var actual = await _service.GetPostcodeAsync<PostalpincodeInModel>("110001");

            Assert.IsType<PostalpincodeInModel>(actual);
            Assert.Equal("Success", actual.Status);
            Assert.Equal("Baroda House", actual.PostOffice[0].Name);
            Assert.Equal("India", actual.PostOffice[0].Country);
        }
    }
}