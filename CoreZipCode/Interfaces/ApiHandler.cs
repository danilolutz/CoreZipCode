using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreZipCode.Interfaces
{
    public class ApiHandler
    {
        public ApiHandler() => Request = new HttpClient();

        public ApiHandler(HttpClient request) => Request = request;

        public HttpClient Request { get; private set; }

        public virtual string CallApi(string url)
        {
            try
            {
                var response = Request.GetAsync(url).Result;

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new ArgumentException();

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error trying execute the request: {ex.Message}");
            }
        }

        public virtual async Task<string> CallApiAsync(string url)
        {
            try
            {
                var response = await Request.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new ArgumentException();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error trying execute the request: {ex.Message}");
            }
        }
    }
}