using CoreZipCode.Interfaces;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoreZipCode.Tests.Interfaces
{
    public class ApiHandlerTest
    {
        [Fact]
        public void Constructor_Without_Parameters_Should_Create_Instance()
        {
            var handler = new ApiHandler();

            Assert.NotNull(handler);
        }

        [Fact]
        public void Constructor_With_HttpClient_Should_Create_Instance()
        {
            var httpClient = new HttpClient();

            var handler = new ApiHandler(httpClient);

            Assert.NotNull(handler);
        }

        [Fact]
        public void Constructor_With_Null_HttpClient_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ApiHandler(null));
        }

        [Fact]
        public async Task CallApiAsync_With_Null_Url_Should_Return_BadRequest()
        {
            var handler = new ApiHandler();

            var result = await handler.CallApiAsync(null);

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.StatusCode);
            Assert.Equal("URL cannot be null or empty.", result.Error.Message);
        }

        [Fact]
        public async Task CallApiAsync_With_Empty_Url_Should_Return_BadRequest()
        {
            var handler = new ApiHandler();

            var result = await handler.CallApiAsync(string.Empty);

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.StatusCode);
            Assert.Equal("URL cannot be null or empty.", result.Error.Message);
        }

        [Fact]
        public async Task CallApiAsync_With_Whitespace_Url_Should_Return_BadRequest()
        {
            var handler = new ApiHandler();

            var result = await handler.CallApiAsync("   ");

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.StatusCode);
            Assert.Equal("URL cannot be null or empty.", result.Error.Message);
        }

        [Fact]
        public async Task CallApiAsync_With_Success_Response_Should_Return_Success()
        {
            var expectedBody = "{\"data\":\"test\"}";
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedBody)
                });
            var httpClient = new HttpClient(handlerMock.Object);
            var handler = new ApiHandler(httpClient);

            var result = await handler.CallApiAsync("https://test.com");

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedBody, result.Value);
        }

        [Fact]
        public async Task CallApiAsync_With_NonSuccess_Response_Should_Return_Failure()
        {
            var responseBody = "{\"error\":\"not found\"}";
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = "Not Found",
                    Content = new StringContent(responseBody)
                });
            var httpClient = new HttpClient(handlerMock.Object);
            var handler = new ApiHandler(httpClient);

            var result = await handler.CallApiAsync("https://test.com");

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.NotFound, result.Error.StatusCode);
            Assert.Contains("404", result.Error.Message);
            Assert.Contains("NotFound", result.Error.Message);
            Assert.Equal(responseBody, result.Error.ResponseBody);
        }

        [Fact]
        public async Task CallApiAsync_With_HttpRequestException_Should_Return_ServiceUnavailable()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Connection failed"));
            var httpClient = new HttpClient(handlerMock.Object);
            var handler = new ApiHandler(httpClient);

            var result = await handler.CallApiAsync("https://test.com");

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, result.Error.StatusCode);
            Assert.Equal("Network or connection error.", result.Error.Message);
        }

        [Fact]
        public async Task CallApiAsync_With_TaskCanceledException_Should_Return_RequestTimeout()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new TaskCanceledException("The request timed out"));
            var httpClient = new HttpClient(handlerMock.Object);
            var handler = new ApiHandler(httpClient);

            var result = await handler.CallApiAsync("https://test.com");

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.RequestTimeout, result.Error.StatusCode);
            Assert.Equal("Request timed out.", result.Error.Message);
        }

        [Fact]
        public async Task CallApiAsync_With_OperationCanceledException_Should_Return_BadRequest()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new OperationCanceledException("Operation was cancelled"));
            var httpClient = new HttpClient(handlerMock.Object);
            var handler = new ApiHandler(httpClient);

            var result = await handler.CallApiAsync("https://test.com");

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.StatusCode);
            Assert.Equal("Request was cancelled.", result.Error.Message);
        }

        [Fact]
        public async Task CallApiAsync_With_Generic_Exception_Should_Return_InternalServerError()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new InvalidOperationException("Something went wrong"));
            var httpClient = new HttpClient(handlerMock.Object);
            var handler = new ApiHandler(httpClient);

            var result = await handler.CallApiAsync("https://test.com");

            Assert.True(result.IsFailure);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Error.StatusCode);
            Assert.Equal("Unexpected error.", result.Error.Message);
        }

        [Fact]
        public async Task CallApiAsync_With_Multiple_Success_Codes_Should_Return_Success()
        {
            var testCases = new[]
            {
                HttpStatusCode.OK,
                HttpStatusCode.Created,
                HttpStatusCode.Accepted,
                HttpStatusCode.NoContent
            };

            foreach (var statusCode in testCases)
            {
                var handlerMock = new Mock<HttpMessageHandler>();
                handlerMock
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>()
                    )
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = statusCode,
                        Content = new StringContent("success")
                    });
                var httpClient = new HttpClient(handlerMock.Object);
                var handler = new ApiHandler(httpClient);

                var result = await handler.CallApiAsync("https://test.com");

                Assert.True(result.IsSuccess);
            }
        }

        [Fact]
        public async Task CallApiAsync_With_Multiple_Error_Codes_Should_Return_Failure()
        {
            var testCases = new[]
            {
                HttpStatusCode.BadRequest,
                HttpStatusCode.Unauthorized,
                HttpStatusCode.Forbidden,
                HttpStatusCode.NotFound,
                HttpStatusCode.InternalServerError,
                HttpStatusCode.BadGateway,
                HttpStatusCode.ServiceUnavailable
            };

            foreach (var statusCode in testCases)
            {
                var handlerMock = new Mock<HttpMessageHandler>();
                handlerMock
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>()
                    )
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = statusCode,
                        Content = new StringContent("error")
                    });
                var httpClient = new HttpClient(handlerMock.Object);
                var handler = new ApiHandler(httpClient);

                var result = await handler.CallApiAsync("https://test.com");

                Assert.True(result.IsFailure);
                Assert.Equal(statusCode, result.Error.StatusCode);
            }
        }
    }
}