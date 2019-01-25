using System;

namespace CoreZipCode.Services.ViaCepApi
{
    public class ViaCepException : Exception
    {
        public ViaCepException(string message = "") : base(message)
        {
            //
        }
    }
}