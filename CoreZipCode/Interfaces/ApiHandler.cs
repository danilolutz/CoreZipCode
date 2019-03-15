using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreZipCode.Interfaces
{
    /// <summary>
    /// ApiHandler class
    /// </summary>
    public class ApiHandler
    {
        /// <summary>
        /// ApiHandler Constructor without parameter.
        /// </summary>
        public ApiHandler() => Request = new HttpClient();

        /// <summary>
        /// ApiHandler Constructor with parameter: request.
        /// </summary>
        /// <param name="request">HttpClient class param to handle with API Servers Connections.</param>
        public ApiHandler(HttpClient request) => Request = request;

        /// <summary>
        /// Http Client Request.
        /// </summary>
        private HttpClient Request { get; set; }

        /// <summary>
        /// Method to execute the api call.
        /// </summary>
        /// <param name="url">Api url to execute</param>
        /// <returns>String Server response</returns>
        public virtual string CallApi(string url)
        {
            try
            {
                var response = Request.GetAsync(url).Result;

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new ArgumentException();
                }

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Error trying execute the request: {ex.Message}");
            }
        }

        /// <summary>
        /// Method to execute the api call async.
        /// </summary>
        /// <param name="url">Api url to execute async</param>
        /// <returns>String Server response</returns>
        public virtual async Task<string> CallApiAsync(string url)
        {
            try
            {
                var response = await Request.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new ArgumentException();
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Error trying execute the request: {ex.Message}");
            }
        }
    }
}