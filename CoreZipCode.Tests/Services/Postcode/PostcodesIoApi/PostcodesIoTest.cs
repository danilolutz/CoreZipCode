using CoreZipCode.Services.Postcode.PostcodesIoApi;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoreZipCode.Tests.Services.Postcode.PostcodesIoApi
{
    public class PostcodesIoTest
    {
        private const string ExpectedResponse = "{\"status\":200,\"result\":[{\"postcode\":\"OX49 5NU\",\"quality\":1,\"eastings\":464447,\"northings\":195647,\"country\":\"England\",\"nhs_ha\":\"South Central\",\"longitude\":-1.069752,\"latitude\":51.655929,\"european_electoral_region\":\"South East\",\"primary_care_trust\":\"Oxfordshire\",\"region\":\"South East\",\"lsoa\":\"South Oxfordshire 011B\",\"msoa\":\"South Oxfordshire 011\",\"incode\":\"5NU\",\"outcode\":\"OX49\",\"parliamentary_constituency\":\"Henley\",\"admin_district\":\"South Oxfordshire\",\"parish\":\"Brightwell Baldwin\",\"admin_county\":\"Oxfordshire\",\"admin_ward\":\"Chalgrove\",\"ced\":\"Chalgrove and Watlington\",\"ccg\":\"NHS Oxfordshire\",\"nuts\":\"Oxfordshire\",\"codes\":{\"admin_district\":\"E07000179\",\"admin_county\":\"E10000025\",\"admin_ward\":\"E05009735\",\"parish\":\"E04008109\",\"parliamentary_constituency\":\"E14000742\",\"ccg\":\"E38000136\",\"ced\":\"E58001238\",\"nuts\":\"UKJ14\"}}]}";
        private const string ExpectedCountry = "England";
        private const string PostalPinCodeTest = "OX49 5NU";
        private const string SendAsync = "SendAsync";
        private const string MockUri = "https://unit.test.com/";
        private const int ExpectedStatusSuccess = 200;
        private const int ExpectedResultQuality = 1;

        private readonly PostcodesIo _service;

        public PostcodesIoTest()
        {
            _service = ConfigureService(ExpectedResponse);
        }

        private static PostcodesIo ConfigureService(string response)
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
                BaseAddress = new Uri(MockUri),
            };

            return new PostcodesIo(httpClient);
        }

        [Fact]
        public static void ConstructorTest()
        {
            var actual = new PostcodesIo();
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
            var actual = _service.GetPostcode<PostcodesIoModel>(PostalPinCodeTest);

            Assert.IsType<PostcodesIoModel>(actual);
            Assert.Equal(ExpectedStatusSuccess, actual.Status);
            Assert.Equal(ExpectedResultQuality, actual.Result[0].Quality);
            Assert.Equal(ExpectedCountry, actual.Result[0].Country);
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
            var actual = await _service.GetPostcodeAsync<PostcodesIoModel>(PostalPinCodeTest);

            Assert.IsType<PostcodesIoModel>(actual);
            Assert.Equal(ExpectedStatusSuccess, actual.Status);
            Assert.Equal(ExpectedResultQuality, actual.Result[0].Quality);
            Assert.Equal(ExpectedCountry, actual.Result[0].Country);
        }
    }
}