using System;

namespace CoreZipCode.Services.ZipCode.SmartyStreetsApi
{
    public class SmartyStreetsException : Exception
    {
        public SmartyStreetsException(string message) : base(message)
        {
            //
        }
    }
}