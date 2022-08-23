using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CoreZipCode.Services.ZipCode.SmartyApi
{
    [Serializable]
    public class SmartyModel
    {
        [JsonProperty("input_index")]
        public long InputIndex { get; set; }

        [JsonProperty("city_states")]
        public List<CityState> CityStates { get; set; }

        [JsonProperty("zipcodes")]
        public List<Zipcode> Zipcodes { get; set; }
    }

    public class CityState
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state_abbreviation")]
        public string StateAbbreviation { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("mailable_city")]
        public bool MailableCity { get; set; }
    }

    public class Zipcode
    {
        [JsonProperty("zipcode")]
        public string ZipcodeNumber { get; set; }

        [JsonProperty("zipcode_type")]
        public string ZipcodeType { get; set; }

        [JsonProperty("default_city")]
        public string DefaultCity { get; set; }

        [JsonProperty("county_fips")]
        public string CountyFips { get; set; }

        [JsonProperty("county_name")]
        public string CountyName { get; set; }

        [JsonProperty("state_abbreviation")]
        public string StateAbbreviation { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("precision")]
        public string Precision { get; set; }
    }
}