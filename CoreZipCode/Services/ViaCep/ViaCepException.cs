using System;

namespace CoreZipCode.Services.ViaCep
{
    public class ViaCepException : Exception
    {
        public ViaCepException(string message = "") : base(message) {
            //
        }
    }
}