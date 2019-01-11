using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CoreZipCode.Services.Postcode.PostalpincodeInApi;
using Moq;
using Moq.Protected;
using Xunit;

namespace CoreZipCode.Tests.Services.Postcode.PostalpincodeInApi
{
    public class PostalpincodeInTest
    {
         private readonly string _expectedResponse = "{\"status\":200,\"result\":[{\"postcode\":\"OX49 5NU\",\"quality\":1,\"eastings\":464447,\"northings\":195647,\"country\":\"England\",\"nhs_ha\":\"South Central\",\"longitude\":-1.069752,\"latitude\":51.655929,\"european_electoral_region\":\"South East\",\"primary_care_trust\":\"Oxfordshire\",\"region\":\"South East\",\"lsoa\":\"South Oxfordshire 011B\",\"msoa\":\"South Oxfordshire 011\",\"incode\":\"5NU\",\"outcode\":\"OX49\",\"parliamentary_constituency\":\"Henley\",\"admin_district\":\"South Oxfordshire\",\"parish\":\"Brightwell Baldwin\",\"admin_county\":\"Oxfordshire\",\"admin_ward\":\"Chalgrove\",\"ced\":\"Chalgrove and Watlington\",\"ccg\":\"NHS Oxfordshire\",\"nuts\":\"Oxfordshire\",\"codes\":{\"admin_district\":\"E07000179\",\"admin_county\":\"E10000025\",\"admin_ward\":\"E05009735\",\"parish\":\"E04008109\",\"parliamentary_constituency\":\"E14000742\",\"ccg\":\"E38000136\",\"ced\":\"E58001238\",\"nuts\":\"UKJ14\"}}]}";
        private Mock<HttpMessageHandler> _handlerMock;
        private PostalpincodeIn _service;

        public PostalpincodeInTest()
        {
            // _service = ConfigureService(_expectedResponse);
            _service = new PostalpincodeIn(); //ConfigureService(_expectedResponse);
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