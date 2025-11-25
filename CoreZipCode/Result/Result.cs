using System;

namespace CoreZipCode.Result
{
    /// <summary>
    /// Immutable value object representing the outcome of an operation.
    /// (no records, no init-only).
    /// </summary>
    public sealed class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public T Value { get; }
        public ApiError Error { get; }

        private Result(T value)
        {
            IsSuccess = true;
            Value = value;
            Error = null!;
        }

        private Result(ApiError error)
        {
            IsSuccess = false;
            Error = error ?? throw new ArgumentNullException(nameof(error));
            Value = default!;
        }

        public static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Failure(ApiError error) => new Result<T>(error);

        public Result<TResult> Map<TResult>(Func<T, TResult> mapper)
        {
            return IsSuccess
                ? Result<TResult>.Success(mapper(Value))
                : Result<TResult>.Failure(Error);
        }

        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<ApiError, TResult> onFailure)
        {
            return IsSuccess ? onSuccess(Value) : onFailure(Error);
        }
    }
}

