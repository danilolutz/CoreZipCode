using Newtonsoft.Json;
using System;

namespace CoreZipCode.Services.ZipCode.ViaCepApi
{
    [Serializable]
    public class ViaCepAddressModel
    {
        [JsonProperty(PropertyName = "cep")]
        public string ZipCode { get; set; }

        [JsonProperty(PropertyName = "logradouro")]
        public string Address1 { get; set; }

        [JsonProperty(PropertyName = "complemento")]
        public string Complement { get; set; }

        [JsonProperty(PropertyName = "bairro")]
        public string Neighborhood { get; set; }

        [JsonProperty(PropertyName = "localidade")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "uf")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "ibge")]
        public string Ibge { get; set; }

        [JsonProperty(PropertyName = "gia")]
        public string Gia { get; set; }

        [JsonProperty(PropertyName = "ddd")]
        public string Ddd { get; set; }

        [JsonProperty(PropertyName = "siafi")]
        public string Siafi { get; set; }
    }
}