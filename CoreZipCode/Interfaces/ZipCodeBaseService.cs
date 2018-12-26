using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace CoreZipCode.Interfaces
{
    public abstract class ZipCodeBaseService
    {
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

        public string Execute(string zipcode) => CallApi(SetZipCodeUrl(zipcode));

        public string Execute(string state, string city, string street) => CallApi(SetZipCodeUrlBy(state, city, street));

        public T GetAddress<T>(string zipcode) => JsonConvert.DeserializeObject<T>(CallApi(SetZipCodeUrl(zipcode)));

        public IList<T> ListAddresses<T>(string state, string city, string street) => JsonConvert.DeserializeObject<IList<T>>(CallApi(SetZipCodeUrlBy(state, city, street)));

        public abstract string SetZipCodeUrl(string zipcode);
        public abstract string SetZipCodeUrlBy(string state, string city, string street);
    }
}