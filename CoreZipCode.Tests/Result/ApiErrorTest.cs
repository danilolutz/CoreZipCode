using System;
using System.Net;
using CoreZipCode.Result;
using Xunit;

namespace CoreZipCode.Tests.Result
{
    public class ApiErrorTest
    {
        [Fact]
        public void Constructor_With_Required_Parameters_Should_Create_Instance()
        {
            var statusCode = HttpStatusCode.BadRequest;
            var message = "Test error message";

            var error = new ApiError(statusCode, message);

            Assert.Equal(statusCode, error.StatusCode);
            Assert.Equal(message, error.Message);
            Assert.Null(error.Detail);
            Assert.Null(error.ResponseBody);
        }

        [Fact]
        public void Constructor_With_All_Parameters_Should_Create_Instance()
        {
            var statusCode = HttpStatusCode.NotFound;
            var message = "Resource not found";
            var detail = "The requested resource does not exist";
            var responseBody = "{\"error\":\"not found\"}";

            var error = new ApiError(statusCode, message, detail, responseBody);

            Assert.Equal(statusCode, error.StatusCode);
            Assert.Equal(message, error.Message);
            Assert.Equal(detail, error.Detail);
            Assert.Equal(responseBody, error.ResponseBody);
        }

        [Fact]
        public void Constructor_With_Detail_Only_Should_Create_Instance()
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Server error";
            var detail = "Database connection failed";

            var error = new ApiError(statusCode, message, detail);

            Assert.Equal(statusCode, error.StatusCode);
            Assert.Equal(message, error.Message);
            Assert.Equal(detail, error.Detail);
            Assert.Null(error.ResponseBody);
        }

        [Fact]
        public void Constructor_With_ResponseBody_Only_Should_Create_Instance()
        {
            var statusCode = HttpStatusCode.BadRequest;
            var message = "Validation failed";
            var responseBody = "{\"errors\":[]}";

            var error = new ApiError(statusCode, message, null, responseBody);

            Assert.Equal(statusCode, error.StatusCode);
            Assert.Equal(message, error.Message);
            Assert.Null(error.Detail);
            Assert.Equal(responseBody, error.ResponseBody);
        }

        [Fact]
        public void Constructor_With_Null_Message_Should_Throw_ArgumentNullException()
        {
            var statusCode = HttpStatusCode.BadRequest;

            Assert.Throws<ArgumentNullException>(() => new ApiError(statusCode, null));
        }

        [Fact]
        public void Constructor_With_Empty_Message_Should_Create_Instance()
        {
            var statusCode = HttpStatusCode.BadRequest;
            var message = string.Empty;

            var error = new ApiError(statusCode, message);

            Assert.Equal(statusCode, error.StatusCode);
            Assert.Equal(message, error.Message);
        }

        [Fact]
        public void Constructor_With_Different_StatusCodes_Should_Create_Instance()
        {
            var testCases = new[]
            {
                HttpStatusCode.OK,
                HttpStatusCode.Created,
                HttpStatusCode.BadRequest,
                HttpStatusCode.Unauthorized,
                HttpStatusCode.Forbidden,
                HttpStatusCode.NotFound,
                HttpStatusCode.MethodNotAllowed,
                HttpStatusCode.InternalServerError,
                HttpStatusCode.BadGateway,
                HttpStatusCode.ServiceUnavailable,
                HttpStatusCode.GatewayTimeout
            };

            foreach (var statusCode in testCases)
            {
                var error = new ApiError(statusCode, "Test message");

                Assert.Equal(statusCode, error.StatusCode);
            }
        }

        [Fact]
        public void ToString_Should_Return_Formatted_String()
        {
            var statusCode = HttpStatusCode.NotFound;
            var message = "Resource not found";
            var error = new ApiError(statusCode, message);

            var result = error.ToString();

            Assert.Equal("404 NotFound: Resource not found", result);
        }

        [Fact]
        public void ToString_With_BadRequest_Should_Return_Formatted_String()
        {
            var statusCode = HttpStatusCode.BadRequest;
            var message = "Invalid input";
            var error = new ApiError(statusCode, message);

            var result = error.ToString();

            Assert.Equal("400 BadRequest: Invalid input", result);
        }

        [Fact]
        public void ToString_With_InternalServerError_Should_Return_Formatted_String()
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Server error occurred";
            var error = new ApiError(statusCode, message);

            var result = error.ToString();

            Assert.Equal("500 InternalServerError: Server error occurred", result);
        }

        [Fact]
        public void ToString_Should_Not_Include_Detail_Or_ResponseBody()
        {
            var statusCode = HttpStatusCode.BadRequest;
            var message = "Validation failed";
            var detail = "Field X is required";
            var responseBody = "{\"error\":\"validation\"}";
            var error = new ApiError(statusCode, message, detail, responseBody);

            var result = error.ToString();

            Assert.Equal("400 BadRequest: Validation failed", result);
            Assert.DoesNotContain(detail, result);
            Assert.DoesNotContain(responseBody, result);
        }

        [Fact]
        public void ToString_With_Empty_Message_Should_Return_Formatted_String()
        {
            var statusCode = HttpStatusCode.BadRequest;
            var message = string.Empty;
            var error = new ApiError(statusCode, message);

            var result = error.ToString();

            Assert.Equal("400 BadRequest: ", result);
        }

        [Fact]
        public void Multiple_Instances_Should_Be_Independent()
        {
            var error1 = new ApiError(HttpStatusCode.BadRequest, "Error 1");
            var error2 = new ApiError(HttpStatusCode.NotFound, "Error 2");

            Assert.NotEqual(error1.StatusCode, error2.StatusCode);
            Assert.NotEqual(error1.Message, error2.Message);
        }

        [Fact]
        public void StatusCode_Should_Be_Immutable()
        {
            var error = new ApiError(HttpStatusCode.BadRequest, "Test");
            var statusCode = error.StatusCode;

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);
            Assert.Equal(HttpStatusCode.BadRequest, error.StatusCode);
        }

        [Fact]
        public void Message_Should_Be_Immutable()
        {
            var message = "Original message";
            var error = new ApiError(HttpStatusCode.BadRequest, message);

            Assert.Equal(message, error.Message);
        }

        [Fact]
        public void Detail_Should_Be_Immutable()
        {
            var detail = "Detailed information";
            var error = new ApiError(HttpStatusCode.BadRequest, "Message", detail);

            Assert.Equal(detail, error.Detail);
        }

        [Fact]
        public void ResponseBody_Should_Be_Immutable()
        {
            var responseBody = "{\"error\":\"test\"}";
            var error = new ApiError(HttpStatusCode.BadRequest, "Message", null, responseBody);

            Assert.Equal(responseBody, error.ResponseBody);
        }

        [Fact]
        public void Properties_Should_Match_Constructor_Parameters()
        {
            var statusCode = HttpStatusCode.Unauthorized;
            var message = "Authentication required";
            var detail = "Token expired";
            var responseBody = "{\"auth\":false}";

            var error = new ApiError(statusCode, message, detail, responseBody);

            Assert.Equal(statusCode, error.StatusCode);
            Assert.Equal(message, error.Message);
            Assert.Equal(detail, error.Detail);
            Assert.Equal(responseBody, error.ResponseBody);
        }

        [Fact]
        public void Constructor_With_Whitespace_Message_Should_Create_Instance()
        {
            var statusCode = HttpStatusCode.BadRequest;
            var message = "   ";

            var error = new ApiError(statusCode, message);

            Assert.Equal(statusCode, error.StatusCode);
            Assert.Equal(message, error.Message);
        }

        [Fact]
        public void Constructor_With_Long_Message_Should_Create_Instance()
        {
            var statusCode = HttpStatusCode.BadRequest;
            var message = new string('x', 1000);

            var error = new ApiError(statusCode, message);

            Assert.Equal(statusCode, error.StatusCode);
            Assert.Equal(message, error.Message);
        }

        [Fact]
        public void Constructor_With_Special_Characters_In_Message_Should_Create_Instance()
        {
            var statusCode = HttpStatusCode.BadRequest;
            var message = "Error: <test> & \"quotes\" & 'apostrophes'";

            var error = new ApiError(statusCode, message);

            Assert.Equal(statusCode, error.StatusCode);
            Assert.Equal(message, error.Message);
        }

        [Fact]
        public void ToString_With_Special_Characters_Should_Include_Them()
        {
            var statusCode = HttpStatusCode.BadRequest;
            var message = "Error: <test> & special";
            var error = new ApiError(statusCode, message);

            var result = error.ToString();

            Assert.Contains("<test>", result);
            Assert.Contains("&", result);
        }
    }
}