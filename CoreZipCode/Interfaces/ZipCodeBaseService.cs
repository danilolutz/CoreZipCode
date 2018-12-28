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
            //
        }

        private string CallApi(string url)
        {
            try
            {
                var request = new HttpClient();
                var response = request.GetAsync(url).Result;

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new ArgumentException();

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error trying execute the request: {ex.Message}");
            }
        }

        private async Task<string> CallApiAsync(string url)
        {
            try
            {
                var request = new HttpClient();
                var response = await request.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new ArgumentException();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error trying execute the request: {ex.Message}");
            }
        }

        public string Execute(string zipcode) => CallApi(SetZipCodeUrl(zipcode));

        public string Execute(string state, string city, string street) => CallApi(SetZipCodeUrlBy(state, city, street));

        public T GetAddress<T>(string zipcode) => JsonConvert.DeserializeObject<T>(CallApi(SetZipCodeUrl(zipcode)));

        public IList<T> ListAddresses<T>(string state, string city, string street) => JsonConvert.DeserializeObject<IList<T>>(CallApi(SetZipCodeUrlBy(state, city, street)));

        public async Task<string> ExecuteAsync(string zipcode) => await CallApiAsync(SetZipCodeUrl(zipcode));

        public async Task<string> ExecuteAsync(string state, string city, string street) => await CallApiAsync(SetZipCodeUrlBy(state, city, street));

        public async Task<T> GetAddressAsync<T>(string zipcode) => JsonConvert.DeserializeObject<T>(await CallApiAsync(SetZipCodeUrl(zipcode)));

        public async Task<IList<T>> ListAddressesAsync<T>(string state, string city, string street) => JsonConvert.DeserializeObject<IList<T>>(await CallApiAsync(SetZipCodeUrlBy(state, city, street)));

        public abstract string SetZipCodeUrl(string zipcode);

        public abstract string SetZipCodeUrlBy(string state, string city, string street);
    }
}