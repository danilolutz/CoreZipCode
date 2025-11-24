using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CoreZipCode.Services.Postcode.PostcodesIoApi
{
    /// <summary>
    /// Represents the response model for the Postcodes.io API, containing the status code and a collection of postcode
    /// results.
    /// </summary>
    /// <remarks>This class is typically used to deserialize JSON responses from the Postcodes.io service. The
    /// structure matches the standard response format returned by the API for postcode queries.</remarks>
    [Serializable]
    public class PostcodesIoModel
    {
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("result")]
        public List<Result> Result { get; set; }
    }

    /// <summary>
    /// Represents detailed information about a UK postcode, including geographic, administrative, and electoral data as
    /// returned by postcode lookup services.  
    /// </summary>
    /// <remarks>This class provides structured access to various attributes associated with a postcode, such
    /// as location coordinates, administrative regions, and health authorities. It is typically used to deserialize
    /// responses from postcode APIs and may contain null or default values for properties where data is unavailable.
    /// Thread safety is not guaranteed; if multiple threads access an instance concurrently, external synchronization
    /// is required.</remarks>
    [Serializable]
    public class Result
    {
        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("quality")]
        public long Quality { get; set; }

        [JsonProperty("eastings")]
        public long Eastings { get; set; }

        [JsonProperty("northings")]
        public long Northings { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("nhs_ha")]
        public string NhsHa { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("european_electoral_region")]
        public string EuropeanElectoralRegion { get; set; }

        [JsonProperty("primary_care_trust")]
        public string PrimaryCareTrust { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("lsoa")]
        public string Lsoa { get; set; }

        [JsonProperty("msoa")]
        public string Msoa { get; set; }

        [JsonProperty("incode")]
        public string Incode { get; set; }

        [JsonProperty("outcode")]
        public string Outcode { get; set; }

        [JsonProperty("parliamentary_constituency")]
        public string ParliamentaryConstituency { get; set; }

        [JsonProperty("admin_district")]
        public string AdminDistrict { get; set; }

        [JsonProperty("parish")]
        public string Parish { get; set; }

        [JsonProperty("admin_county")]
        public object AdminCounty { get; set; }

        [JsonProperty("admin_ward")]
        public string AdminWard { get; set; }

        [JsonProperty("ced")]
        public object Ced { get; set; }

        [JsonProperty("ccg")]
        public string Ccg { get; set; }

        [JsonProperty("nuts")]
        public string Nuts { get; set; }

        [JsonProperty("codes")]
        public Codes Codes { get; set; }
    }

    /// <summary>
    /// Represents a collection of administrative and geographic codes associated with a location, such as district,
    /// county, ward, parish, and constituency identifiers. 
    /// </summary>
    /// <remarks>This class is typically used to provide standardized codes for various administrative
    /// divisions in the United Kingdom. The codes correspond to specific government or statistical regions and can be
    /// used for data integration, analysis, or mapping purposes. All properties are optional and may be null if the
    /// corresponding code is not available.</remarks>
    [Serializable]
    public class Codes
    {
        [JsonProperty("admin_district")]
        public string AdminDistrict { get; set; }

        [JsonProperty("admin_county")]
        public string AdminCounty { get; set; }

        [JsonProperty("admin_ward")]
        public string AdminWard { get; set; }

        [JsonProperty("parish")]
        public string Parish { get; set; }

        [JsonProperty("parliamentary_constituency")]
        public string ParliamentaryConstituency { get; set; }

        [JsonProperty("ccg")]
        public string Ccg { get; set; }

        [JsonProperty("ced")]
        public string Ced { get; set; }

        [JsonProperty("nuts")]
        public string Nuts { get; set; }
    }
}
