using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreZipCode.Interfaces
{
    public abstract class ZipCodeBaseService
    {
        public ZipCodeBaseService()
        {
            Request = new HttpClient();
        }

        public ZipCodeBaseService(HttpClient request)
        {
            Request = request;
        }

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

        public virtual string Execute(string zipcode) => CallApi(SetZipCodeUrl(zipcode));

        public virtual string Execute(string state, string city, string street) => CallApi(SetZipCodeUrlBy(state, city, street));

        public virtual T GetAddress<T>(string zipcode) => JsonConvert.DeserializeObject<T>(CallApi(SetZipCodeUrl(zipcode)));

        public virtual IList<T> ListAddresses<T>(string state, string city, string street) => JsonConvert.DeserializeObject<IList<T>>(CallApi(SetZipCodeUrlBy(state, city, street)));

        public virtual async Task<string> ExecuteAsync(string zipcode) => await CallApiAsync(SetZipCodeUrl(zipcode));

        public virtual async Task<string> ExecuteAsync(string state, string city, string street) => await CallApiAsync(SetZipCodeUrlBy(state, city, street));

        public virtual async Task<T> GetAddressAsync<T>(string zipcode) => JsonConvert.DeserializeObject<T>(await CallApiAsync(SetZipCodeUrl(zipcode)));

        public virtual async Task<IList<T>> ListAddressesAsync<T>(string state, string city, string street) => JsonConvert.DeserializeObject<IList<T>>(await CallApiAsync(SetZipCodeUrlBy(state, city, street)));

        public abstract string SetZipCodeUrl(string zipcode);

        public abstract string SetZipCodeUrlBy(string state, string city, string street);
    }
}