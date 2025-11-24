using System;

namespace CoreZipCode.Services.ZipCode.SmartyApi
{
    /// <summary>
    /// Represents errors that occur during Smarty operations.  
    /// </summary>
    /// <remarks>Use this exception to indicate failures specific to Smarty functionality. This exception is
    /// typically thrown when an operation cannot be completed due to invalid input, configuration issues, or other
    /// conditions unique to Smarty. For general exceptions, use the base <see cref="Exception"/> class.</remarks>
    public class SmartyException : Exception
    {
        public SmartyException(string message) : base(message)
        {
            //
        }
    }
}