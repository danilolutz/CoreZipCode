using CoreZipCode.Interfaces;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoreZipCode.Tests.Services.Interfaces
{
    public class ApiHandlerTest
    {
        private Mock<HttpMessageHandler> _handlerMock;

        private HttpClient ConfigureService(HttpStatusCode statusCode)
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
                    StatusCode = statusCode,
                    Content = new StringContent(""),
                })
                .Verifiable();

            var httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("https://unit.test.com/"),
            };

            return httpClient;
        }

        [Fact]
        public void MustCreateANewInstance()
        {
            var apiHandler = new ApiHandler();
            var apiHandlerWithHttpClient = new ApiHandler(ConfigureService(HttpStatusCode.OK));

            Assert.IsType<ApiHandler>(apiHandler);
            Assert.IsType<ApiHandler>(apiHandlerWithHttpClient);
        }

        [Fact]
        public void MustCallApiException()
        {
            var apiHandler = new ApiHandler(ConfigureService(HttpStatusCode.BadRequest));

            Assert.Throws<Exception>(() => apiHandler.CallApi("https://unit.test.com/"));
        }

        [Fact]
        public async void MustCallApiAsyncException()
        {
            var apiHandler = new ApiHandler(ConfigureService(HttpStatusCode.BadRequest));

            await Assert.ThrowsAsync<Exception>(() => apiHandler.CallApiAsync("https://unit.test.com/"));
        }
    }
}