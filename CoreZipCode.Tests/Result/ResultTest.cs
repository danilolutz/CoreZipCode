using System;
using System.Net;
using CoreZipCode.Result;
using Xunit;

namespace CoreZipCode.Tests.Result
{
    public class ResultTest
    {
        [Fact]
        public void Success_Should_Create_Success_Result()
        {
            var value = "test data";

            var result = Result<string>.Success(value);

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Equal(value, result.Value);
            Assert.Null(result.Error);
        }

        [Fact]
        public void Success_With_Null_Value_Should_Create_Success_Result()
        {
            var result = Result<string>.Success(null);

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Null(result.Value);
            Assert.Null(result.Error);
        }

        [Fact]
        public void Success_With_Int_Should_Create_Success_Result()
        {
            var value = 42;

            var result = Result<int>.Success(value);

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void Success_With_Object_Should_Create_Success_Result()
        {
            var value = new { Id = 1, Name = "Test" };

            var result = Result<object>.Success(value);

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void Failure_Should_Create_Failure_Result()
        {
            var error = new ApiError(HttpStatusCode.BadRequest, "Error message");

            var result = Result<string>.Failure(error);

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal(error, result.Error);
            Assert.Null(result.Value);
        }

        [Fact]
        public void Failure_With_Null_Error_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Result<string>.Failure(null));
        }

        [Fact]
        public void Failure_With_Different_Error_Codes_Should_Create_Failure_Result()
        {
            var testCases = new[]
            {
                HttpStatusCode.NotFound,
                HttpStatusCode.InternalServerError,
                HttpStatusCode.Unauthorized,
                HttpStatusCode.Forbidden
            };

            foreach (var statusCode in testCases)
            {
                var error = new ApiError(statusCode, "Error");

                var result = Result<string>.Failure(error);

                Assert.True(result.IsFailure);
                Assert.Equal(error, result.Error);
                Assert.Equal(statusCode, result.Error.StatusCode);
            }
        }

        [Fact]
        public void Map_On_Success_Should_Transform_Value()
        {
            var result = Result<int>.Success(5);

            var mapped = result.Map(x => x * 2);

            Assert.True(mapped.IsSuccess);
            Assert.Equal(10, mapped.Value);
        }

        [Fact]
        public void Map_On_Success_Should_Change_Type()
        {
            var result = Result<int>.Success(42);

            var mapped = result.Map(x => x.ToString());

            Assert.True(mapped.IsSuccess);
            Assert.Equal("42", mapped.Value);
        }

        [Fact]
        public void Map_On_Failure_Should_Propagate_Error()
        {
            var error = new ApiError(HttpStatusCode.BadRequest, "Original error");
            var result = Result<int>.Failure(error);

            var mapped = result.Map(x => x * 2);

            Assert.False(mapped.IsSuccess);
            Assert.True(mapped.IsFailure);
            Assert.Equal(error, mapped.Error);
            Assert.Equal(HttpStatusCode.BadRequest, mapped.Error.StatusCode);
            Assert.Equal("Original error", mapped.Error.Message);
        }

        [Fact]
        public void Map_On_Failure_Should_Not_Execute_Mapper()
        {
            var error = new ApiError(HttpStatusCode.BadRequest, "Error");
            var result = Result<int>.Failure(error);
            var mapperExecuted = false;

            var mapped = result.Map(x =>
            {
                mapperExecuted = true;
                return x * 2;
            });

            Assert.False(mapperExecuted);
            Assert.True(mapped.IsFailure);
        }

        [Fact]
        public void Map_Chain_On_Success_Should_Transform_Multiple_Times()
        {
            var result = Result<int>.Success(5);

            var mapped = result
                .Map(x => x * 2)
                .Map(x => x + 10)
                .Map(x => x.ToString());

            Assert.True(mapped.IsSuccess);
            Assert.Equal("20", mapped.Value);
        }

        [Fact]
        public void Map_Chain_On_Failure_Should_Propagate_Error_Through_Chain()
        {
            var error = new ApiError(HttpStatusCode.BadRequest, "Error");
            var result = Result<int>.Failure(error);

            var mapped = result
                .Map(x => x * 2)
                .Map(x => x + 10)
                .Map(x => x.ToString());

            Assert.True(mapped.IsFailure);
            Assert.Equal(error, mapped.Error);
        }

        [Fact]
        public void Match_On_Success_Should_Execute_OnSuccess()
        {
            var result = Result<int>.Success(42);

            var output = result.Match(
                onSuccess: value => $"Success: {value}",
                onFailure: error => $"Failure: {error.Message}"
            );

            Assert.Equal("Success: 42", output);
        }

        [Fact]
        public void Match_On_Failure_Should_Execute_OnFailure()
        {
            var error = new ApiError(HttpStatusCode.BadRequest, "Bad request");
            var result = Result<int>.Failure(error);

            var output = result.Match(
                onSuccess: value => $"Success: {value}",
                onFailure: err => $"Failure: {err.Message}"
            );

            Assert.Equal("Failure: Bad request", output);
        }

        [Fact]
        public void Match_On_Success_Should_Not_Execute_OnFailure()
        {
            var result = Result<int>.Success(42);
            var onFailureExecuted = false;

            result.Match(
                onSuccess: value => value * 2,
                onFailure: error =>
                {
                    onFailureExecuted = true;
                    return 0;
                }
            );

            Assert.False(onFailureExecuted);
        }

        [Fact]
        public void Match_On_Failure_Should_Not_Execute_OnSuccess()
        {
            var error = new ApiError(HttpStatusCode.BadRequest, "Error");
            var result = Result<int>.Failure(error);
            var onSuccessExecuted = false;

            result.Match(
                onSuccess: value =>
                {
                    onSuccessExecuted = true;
                    return value;
                },
                onFailure: err => 0
            );

            Assert.False(onSuccessExecuted);
        }

        [Fact]
        public void Match_Should_Return_Correct_Type()
        {
            var result = Result<string>.Success("test");

            var length = result.Match(
                onSuccess: value => value.Length,
                onFailure: error => -1
            );

            Assert.Equal(4, length);
        }

        [Fact]
        public void Match_With_Different_Return_Types_Should_Work()
        {
            var successResult = Result<int>.Success(42);
            var failureResult = Result<int>.Failure(new ApiError(HttpStatusCode.BadRequest, "Error"));

            var successOutput = successResult.Match(
                onSuccess: value => (true, value),
                onFailure: error => (false, 0)
            );

            var failureOutput = failureResult.Match(
                onSuccess: value => (true, value),
                onFailure: error => (false, 0)
            );

            Assert.True(successOutput.Item1);
            Assert.Equal(42, successOutput.Item2);
            Assert.False(failureOutput.Item1);
            Assert.Equal(0, failureOutput.Item2);
        }

        [Fact]
        public void IsFailure_Should_Be_Opposite_Of_IsSuccess()
        {
            var success = Result<int>.Success(42);
            var failure = Result<int>.Failure(new ApiError(HttpStatusCode.BadRequest, "Error"));

            Assert.True(success.IsSuccess);
            Assert.False(success.IsFailure);
            Assert.False(failure.IsSuccess);
            Assert.True(failure.IsFailure);
        }

        [Fact]
        public void Multiple_Success_Results_Should_Be_Independent()
        {
            var result1 = Result<int>.Success(1);
            var result2 = Result<int>.Success(2);

            Assert.Equal(1, result1.Value);
            Assert.Equal(2, result2.Value);
            Assert.NotEqual(result1.Value, result2.Value);
        }

        [Fact]
        public void Multiple_Failure_Results_Should_Be_Independent()
        {
            var error1 = new ApiError(HttpStatusCode.BadRequest, "Error 1");
            var error2 = new ApiError(HttpStatusCode.NotFound, "Error 2");
            var result1 = Result<int>.Failure(error1);
            var result2 = Result<int>.Failure(error2);

            Assert.Equal(error1, result1.Error);
            Assert.Equal(error2, result2.Error);
            Assert.NotEqual(result1.Error, result2.Error);
        }
    }
}