using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreZipCode.Interfaces
{
    /// <summary>
    /// Zip code base service abstract class
    /// </summary>
    public abstract class ZipCodeBaseService : ApiHandler
    {
        /// <summary>
        /// Zip Code Base Service protected constructor.
        /// </summary>
        /// <param name="request">HttpClient Request</param>
        protected ZipCodeBaseService(HttpClient request) : base(request) { }

        /// <summary>
        /// Execute the address query by zip code.
        /// </summary>
        /// <param name="zipCode">Zip code</param>
        /// <returns>String server response</returns>
        public virtual string Execute(string zipCode) => CallApi(SetZipCodeUrl(zipCode));

        /// <summary>
        /// Execute the address query by parameters: state, city, street.
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="city">City</param>
        /// <param name="street">Street</param>
        /// <returns>String server response</returns>
        public virtual string Execute(string state, string city, string street) => CallApi(SetZipCodeUrlBy(state, city, street));

        /// <summary>
        /// Execute the address query by zip code, filling the generic object.
        /// </summary>
        /// <typeparam name="T">Generic object parameter</typeparam>
        /// <param name="zipCode">Zip code</param>
        /// <returns>Generic object filled with postal code</returns>
        public virtual T GetAddress<T>(string zipCode) => JsonConvert.DeserializeObject<T>(CallApi(SetZipCodeUrl(zipCode)));

        /// <summary>
        /// Execute the address list query by parameters: state, city, street. filling the generic object list.
        /// </summary>
        /// <typeparam name="T">Generic object parameter</typeparam>
        /// <param name="state">State</param>
        /// <param name="city">City</param>
        /// <param name="street">Street</param>
        /// <returns>Generic object list filled with zip code</returns>
        public virtual IList<T> ListAddresses<T>(string state, string city, string street) => JsonConvert.DeserializeObject<IList<T>>(CallApi(SetZipCodeUrlBy(state, city, street)));

        /// <summary>
        /// Execute the address query async by zip code.
        /// </summary>
        /// <param name="zipCode">Zip code</param>
        /// <returns>String server response</returns>
        public virtual async Task<string> ExecuteAsync(string zipCode) => await CallApiAsync(SetZipCodeUrl(zipCode));

        /// <summary>
        /// Execute the address query async by parameters: state, city, street.
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="city">City</param>
        /// <param name="street">Street</param>
        /// <returns>string in json format filled with zip code</returns>
        public virtual async Task<string> ExecuteAsync(string state, string city, string street) => await CallApiAsync(SetZipCodeUrlBy(state, city, street));

        /// <summary>
        /// Run the address query by asynchronous zip code, filling in the generic object.
        /// </summary>
        /// <typeparam name="T">Generic object parameter</typeparam>
        /// <param name="zipCode">Zip code</param>
        /// <returns>Generic object filled with postal code</returns>
        public virtual async Task<T> GetAddressAsync<T>(string zipCode) => JsonConvert.DeserializeObject<T>(await CallApiAsync(SetZipCodeUrl(zipCode)));
        
        /// <summary>
        /// Run the address query by asynchronous zip code, filling in the generic object list.
        /// </summary>
        /// <typeparam name="T">Generic object parameter</typeparam>
        /// <param name="state">State</param>
        /// <param name="city">City</param>
        /// <param name="street">Street</param>
        /// <returns>Generic object list filled with zip code</returns>
        public virtual async Task<IList<T>> ListAddressesAsync<T>(string state, string city, string street) => JsonConvert.DeserializeObject<IList<T>>(await CallApiAsync(SetZipCodeUrlBy(state, city, street)));

        /// <summary>
        /// Method for implementation to mount the api url of query by postal code.
        /// </summary>
        /// <param name="zipCode">Zip code</param>
        /// <returns>Url to get api by postal code</returns>
        public abstract string SetZipCodeUrl(string zipCode);

        /// <summary>
        /// Method for implementation to mount the api url of query by parameters: state, city, street.
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="city">City</param>
        /// <param name="street">Street</param>
        /// <returns>Url to get api by parameters: state, city, street</returns>
        public abstract string SetZipCodeUrlBy(string state, string city, string street);
    }
}