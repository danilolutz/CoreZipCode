using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreZipCode.Interfaces
{
    public abstract class ZipCodeBaseService : ApiHandler
    {
        public ZipCodeBaseService(HttpClient request) : base(request) { }

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