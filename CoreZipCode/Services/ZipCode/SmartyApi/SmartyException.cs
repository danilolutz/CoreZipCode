using System;

namespace CoreZipCode.Services.ZipCode.SmartyApi
{
    public class SmartyException : Exception
    {
        public SmartyException(string message) : base(message)
        {
            //
        }
    }
}