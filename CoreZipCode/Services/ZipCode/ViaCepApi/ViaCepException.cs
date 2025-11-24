using System;

namespace CoreZipCode.Services.ZipCode.ViaCepApi
{
    /// <summary>
    /// Represents errors that occur when interacting with the ViaCep API.
    /// </summary>
    /// <remarks>Use this exception to identify and handle failures specific to ViaCep operations, such as
    /// invalid responses or connectivity issues. This exception is typically thrown when the ViaCep service returns an
    /// error or cannot be reached. For general exceptions, use the base <see cref="Exception"/> class.</remarks>
    public class ViaCepException : Exception
    {
        public ViaCepException(string message) : base(message)
        {
            //
        }
    }
}