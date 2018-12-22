using System;
using System.Net;
using System.Net.Http;

namespace CoreZipCode.Interfaces
{
    public abstract class ZipCodeBaseService
    {
        private string Url { get; set; }
        
        private string Execute()
        {
            try
            {
                var request = new HttpClient();
                var response = request.GetAsync(Url).Result;

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new ArgumentException();

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception($"Error trying execute the request: {ex.Message}");
            }
        }
        
        public string Execute(string zipcode)
        {
            Url = SetZipCodeUrl(zipcode);
            return Execute();
        }

        public string Execute(string uf, string city, string street)
        {
            Url = SetZipCodeUrlBy(uf, city, street);
            return Execute();
        }

        public abstract string SetZipCodeUrl(string zipcode);
        public abstract string SetZipCodeUrlBy(string uf, string city, string street);
    }
}