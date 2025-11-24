using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CoreZipCode.Services.ZipCode.SmartyApi
{
    /// <summary>
    /// Represents the result of a Smarty address lookup, including input index and associated city-state and ZIP code
    /// data.
    /// </summary>
    /// <remarks>This model is typically used to deserialize responses from Smarty APIs. It contains
    /// collections of city-state and ZIP code information corresponding to the input address. The class is serializable
    /// and designed for use with JSON serialization frameworks.</remarks>
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

    /// <summary>
    /// Represents a city and its associated state information, including mailing eligibility.  
    /// </summary>
    /// <remarks>This class is typically used to model address components for postal or location-based
    /// operations. The properties provide both the full state name and its abbreviation, as well as an indicator of
    /// whether the city is recognized for mailing purposes.</remarks>
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

    /// <summary>
    /// Represents a United States postal ZIP code and its associated geographic and administrative information.    
    /// </summary>
    /// <remarks>This class provides properties for common ZIP code attributes, including location
    /// coordinates, city, county, and state details. It is typically used to model ZIP code data retrieved from
    /// external sources or APIs. All properties are read-write and correspond to standard ZIP code metadata
    /// fields.</remarks>
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