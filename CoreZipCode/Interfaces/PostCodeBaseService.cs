using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreZipCode.Interfaces
{
    public abstract class PostcodeBaseService : ApiHandler
    {
        protected PostcodeBaseService(HttpClient request) : base(request) { }

        public virtual string Execute(string postcode) => CallApi(SetPostcodeUrl(postcode));

        public virtual async Task<string> ExecuteAsync(string postcode) => await CallApiAsync(SetPostcodeUrl(postcode));

        public virtual T GetPostcode<T>(string postcode) => JsonConvert.DeserializeObject<T>(CallApi(SetPostcodeUrl(postcode)));

        public virtual async Task<T> GetPostcodeAsync<T>(string postcode) => JsonConvert.DeserializeObject<T>(await CallApiAsync(SetPostcodeUrl(postcode)));

        public abstract string SetPostcodeUrl(string postcode);
    }
}