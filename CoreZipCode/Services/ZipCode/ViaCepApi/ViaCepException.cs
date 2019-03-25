using System;

namespace CoreZipCode.Services.ZipCode.ViaCepApi
{
    public class ViaCepException : Exception
    {
        public ViaCepException(string message) : base(message)
        {
            //
        }
    }
}