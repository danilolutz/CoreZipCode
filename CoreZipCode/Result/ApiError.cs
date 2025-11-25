#nullable enable
using System;
using System.Net;

namespace CoreZipCode.Result
{
    /// <summary>
    /// Immutable representation of an API failure.
    /// </summary>
    public sealed class ApiError
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }
        public string? Detail { get; }
        public string? ResponseBody { get; }

        public ApiError(HttpStatusCode statusCode, string message, string? detail = null, string? responseBody = null)
        {
            StatusCode = statusCode;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Detail = detail;
            ResponseBody = responseBody;
        }

        public override string ToString() => $"{(int)StatusCode} {StatusCode}: {Message}";
    }
}