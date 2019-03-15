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
        private const string SendAsync = "SendAsync";
        private const string MockUri = "https://unit.test.com/";

        private static HttpClient ConfigureService(HttpStatusCode statusCode)
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
                    StatusCode = statusCode,
                    Content = new StringContent(""),
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(MockUri),
            };

            return httpClient;
        }

        [Fact]
        public static void MustCreateANewInstance()
        {
            var apiHandler = new ApiHandler();
            var apiHandlerWithHttpClient = new ApiHandler(ConfigureService(HttpStatusCode.OK));

            Assert.IsType<ApiHandler>(apiHandler);
            Assert.IsType<ApiHandler>(apiHandlerWithHttpClient);
        }

        [Fact]
        public static void MustCallApiException()
        {
            var apiHandler = new ApiHandler(ConfigureService(HttpStatusCode.BadRequest));

            Assert.Throws<HttpRequestException>(() => apiHandler.CallApi(MockUri));
        }

        [Fact]
        public static async Task MustCallApiAsyncException()
        {
            var apiHandler = new ApiHandler(ConfigureService(HttpStatusCode.BadRequest));

            await Assert.ThrowsAsync<HttpRequestException>(() => apiHandler.CallApiAsync(MockUri));
        }
    }
}