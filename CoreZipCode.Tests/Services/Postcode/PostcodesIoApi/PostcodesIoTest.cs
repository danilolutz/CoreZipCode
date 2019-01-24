using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CoreZipCode.Services.Postcode.PostcodesIoApi;
using Moq;
using Moq.Protected;
using Xunit;

namespace CoreZipCode.Tests.Services.Postcode.PostcodesIoApi
{
    public class PostcodesIoTest
    {
        private readonly string _expectedResponse = "{\"status\":200,\"result\":[{\"postcode\":\"OX49 5NU\",\"quality\":1,\"eastings\":464447,\"northings\":195647,\"country\":\"England\",\"nhs_ha\":\"South Central\",\"longitude\":-1.069752,\"latitude\":51.655929,\"european_electoral_region\":\"South East\",\"primary_care_trust\":\"Oxfordshire\",\"region\":\"South East\",\"lsoa\":\"South Oxfordshire 011B\",\"msoa\":\"South Oxfordshire 011\",\"incode\":\"5NU\",\"outcode\":\"OX49\",\"parliamentary_constituency\":\"Henley\",\"admin_district\":\"South Oxfordshire\",\"parish\":\"Brightwell Baldwin\",\"admin_county\":\"Oxfordshire\",\"admin_ward\":\"Chalgrove\",\"ced\":\"Chalgrove and Watlington\",\"ccg\":\"NHS Oxfordshire\",\"nuts\":\"Oxfordshire\",\"codes\":{\"admin_district\":\"E07000179\",\"admin_county\":\"E10000025\",\"admin_ward\":\"E05009735\",\"parish\":\"E04008109\",\"parliamentary_constituency\":\"E14000742\",\"ccg\":\"E38000136\",\"ced\":\"E58001238\",\"nuts\":\"UKJ14\"}}]}";
        private Mock<HttpMessageHandler> _handlerMock;
        private PostcodesIo _service;

        public PostcodesIoTest()
        {
            _service = ConfigureService(_expectedResponse);
        }

        private PostcodesIo ConfigureService(string response)
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

            return new PostcodesIo(httpClient);
        }

        [Fact]
        public void MustGetPostCodes()
        {
            var actual = _service.Execute("OX49 5NU");

            Assert.Equal(_expectedResponse, actual);
        }

        [Fact]
        public void MustGetPostCodesObject()
        {
            var actual = _service.GetPostcode<PostcodesIoModel>("OX49 5NU");

            Assert.IsType<PostcodesIoModel>(actual);
            Assert.Equal(200, actual.Status);
            Assert.Equal(1, actual.Result[0].Quality);
            Assert.Equal("England", actual.Result[0].Country);
        }

        [Fact]
        public async void MustGetPostCodesAsync()
        {
            var actual = await _service.ExecuteAsync("OX49 5NU");

            Assert.Equal(_expectedResponse, actual);
        }

        [Fact]
        public async void MustGetPostCodesObjectAsync()
        {
            var actual = await _service.GetPostcodeAsync<PostcodesIoModel>("OX49 5NU");

            Assert.IsType<PostcodesIoModel>(actual);
            Assert.Equal(200, actual.Status);
            Assert.Equal(1, actual.Result[0].Quality);
            Assert.Equal("England", actual.Result[0].Country);
        }
    }
}