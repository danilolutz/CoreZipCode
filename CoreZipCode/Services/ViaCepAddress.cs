using System;
using Newtonsoft.Json;

namespace CoreZipCode.Services
{
    [Serializable]
    public class ViaCepAddress
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

        [JsonProperty(PropertyName = "unidade")]
        public string Unity { get; set; }

        [JsonProperty(PropertyName = "ibge")]
        public string IBGE { get; set; }

        [JsonProperty(PropertyName = "gia")]
        public string GIA { get; set; }
    }
}