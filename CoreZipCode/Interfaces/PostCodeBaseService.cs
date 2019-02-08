using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreZipCode.Interfaces
{
    /// <inheritdoc />
    /// <summary>
    /// Postcode base service abstract class.
    /// </summary>
    public abstract class PostcodeBaseService : ApiHandler
    {
        /// <inheritdoc />
        /// <summary>
        /// postalcode base service protected constructor.
        /// </summary>
        /// <param name="request">HttpClient class param to handle with API Servers Connections.</param>
        protected PostcodeBaseService(HttpClient request) : base(request) { }

        /// <summary>
        /// Execute the address query by zip code.
        /// </summary>
        /// <param name="postcode">Postcode to find for</param>
        /// <returns>String server response</returns>
        public virtual string Execute(string postcode) => CallApi(SetPostcodeUrl(postcode));

        /// <summary>
        /// Execute the address query async by zip code.
        /// </summary>
        /// <param name="postcode">Postcode to find for</param>
        /// <returns>String server response</returns>
        public virtual async Task<string> ExecuteAsync(string postcode) => await CallApiAsync(SetPostcodeUrl(postcode));

        /// <summary>
        /// Run the address query by zip code, filling in the generic object.
        /// </summary>
        /// <typeparam name="T">Generic object parameter</typeparam>
        /// <param name="postcode">Postcode to find for</param>
        /// <returns>String server response</returns>
        public virtual T GetPostcode<T>(string postcode) => JsonConvert.DeserializeObject<T>(CallApi(SetPostcodeUrl(postcode)));

        /// <summary>
        /// Run the address query by asynchronous zip code, filling in the generic object.
        /// </summary>
        /// <typeparam name="T">Generic object parameter</typeparam>
        /// <param name="postcode">Postcode to find for</param>
        /// <returns>String server response</returns>
        public virtual async Task<T> GetPostcodeAsync<T>(string postcode) => JsonConvert.DeserializeObject<T>(await CallApiAsync(SetPostcodeUrl(postcode)));

        /// <summary>
        /// Method for implementation to mount the api url of query by postal code.
        /// </summary>
        /// <param name="postcode">Postcode to find for</param>
        /// <returns>Url to get api by postal code</returns>
        public abstract string SetPostcodeUrl(string postcode);
    }
}