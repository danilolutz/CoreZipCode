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
        private const string ExpectedResponse = "{\"Message\":\"Number of Post office(s) found: 21\",\"Status\":\"Success\",\"PostOffice\":[{\"Name\":\"Baroda House\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Bengali Market\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Bhagat Singh Market\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Connaught Place\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Constitution House\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Election Commission\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Janpath\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Krishi Bhawan\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Lady Harding Medical College\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"New Delhi \",\"Description\":\"\",\"BranchType\":\"Head Post Office\",\"DeliveryStatus\":\"Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"New Delhi\",\"Division\":\"New Delhi GPO\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"North Avenue\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Parliament House\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Patiala House\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Pragati Maidan\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Pragati Maidan Camp\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Rail Bhawan\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Sansad Marg\",\"Description\":\"\",\"BranchType\":\"Head Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Sansadiya Soudh\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Secretariat North\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Shastri Bhawan\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"},{\"Name\":\"Supreme Court\",\"Description\":\"\",\"BranchType\":\"Sub Post Office\",\"DeliveryStatus\":\"Non-Delivery\",\"Taluk\":\"New Delhi\",\"Circle\":\"New Delhi\",\"District\":\"Central Delhi\",\"Division\":\"New Delhi Central\",\"Region\":\"Delhi\",\"State\":\"Delhi\",\"Country\":\"India\"}]}";
        private const string ExpectedStatusSuccess = "Success";
        private const string ExpectedPostOffice = "Baroda House";
        private const string ExpectedCountry = "India";
        private const string PostalPinCodeTest = "110001";
        private const string SendAsync = "SendAsync";
        private const string MockUri = "https://unit.test.com/";

        private readonly PostalpincodeIn _service;

        public PostalpincodeInTest()
        {
            _service = ConfigureService(ExpectedResponse);
        }

        private static PostalpincodeIn ConfigureService(string response)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
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

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(MockUri),
            };

            return new PostalpincodeIn(httpClient);
        }
        
        [Fact]
        public void ConstructorTest()
        {
            var actual = new PostalpincodeIn();
            Assert.NotNull(actual);
        }

        [Fact]
        public void MustGetPostcodes()
        {
            var actual = _service.Execute(PostalPinCodeTest);

            Assert.Equal(ExpectedResponse, actual);
        }

        [Fact]
        public void MustGetPostcodesObject()
        {
            var actual = _service.GetPostcode<PostalpincodeInModel>(PostalPinCodeTest);

            Assert.IsType<PostalpincodeInModel>(actual);
            Assert.Equal(ExpectedStatusSuccess, actual.Status);
            Assert.Equal(ExpectedPostOffice, actual.PostOffice[0].Name);
            Assert.Equal(ExpectedCountry, actual.PostOffice[0].Country);
        }

        [Fact]
        public async Task MustGetPostcodesAsync()
        {
            var actual = await _service.ExecuteAsync(PostalPinCodeTest);

            Assert.Equal(ExpectedResponse, actual);
        }

        [Fact]
        public async Task MustGetPostcodesObjectAsync()
        {
            var actual = await _service.GetPostcodeAsync<PostalpincodeInModel>(PostalPinCodeTest);

            Assert.IsType<PostalpincodeInModel>(actual);
            Assert.Equal(ExpectedStatusSuccess, actual.Status);
            Assert.Equal(ExpectedPostOffice, actual.PostOffice[0].Name);
            Assert.Equal(ExpectedCountry, actual.PostOffice[0].Country);
        }
    }
}