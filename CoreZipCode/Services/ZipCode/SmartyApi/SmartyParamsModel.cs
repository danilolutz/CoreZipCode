using Newtonsoft.Json;

namespace CoreZipCode.Services.ZipCode.SmartyApi
{
    /// <summary>
    /// Represents the address candidate parameters returned by the Smarty address validation API.
    /// </summary>
    /// <remarks>This model contains detailed information about a validated address, including its components,
    /// metadata, and analysis results. It is typically used to access parsed address fields and related data after a
    /// Smarty address lookup operation.</remarks>
    public class SmartyParamsModel
    {
        [JsonProperty("input_index")]
        public long InputIndex { get; set; }

        [JsonProperty("candidate_index")]
        public long CandidateIndex { get; set; }

        [JsonProperty("delivery_line_1")]
        public string DeliveryLine1 { get; set; }

        [JsonProperty("last_line")]
        public string LastLine { get; set; }

        [JsonProperty("delivery_point_barcode")]
        public string DeliveryPointBarcode { get; set; }

        [JsonProperty("components")]
        public Components Components { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("analysis")]
        public Analysis Analysis { get; set; }
    }

    /// <summary>
    /// Represents the results of address analysis, including DPV (Delivery Point Validation) codes and related status
    /// indicators. 
    /// </summary>
    /// <remarks>This class provides properties for various USPS DPV analysis codes and flags, which can be
    /// used to interpret the deliverability and status of an address. The property values correspond to codes returned
    /// by address validation services and may require reference to USPS documentation for detailed meaning.</remarks>
    public class Analysis
    {
        [JsonProperty("dpv_match_code")]
        public string DpvMatchCode { get; set; }

        [JsonProperty("dpv_footnotes")]
        public string DpvFootnotes { get; set; }

        [JsonProperty("dpv_cmra")]
        public string DpvCmra { get; set; }

        [JsonProperty("dpv_vacant")]
        public string DpvVacant { get; set; }

        [JsonProperty("active")]
        public string Active { get; set; }
    }

    /// <summary>
    /// Represents the individual address components for a United States postal address, including street, city, state,
    /// and ZIP code information.   
    /// </summary>
    /// <remarks>This class provides structured properties for each part of an address, which can be used for
    /// address parsing, validation, or formatting. All properties correspond to standard USPS address fields and are
    /// mapped to their respective JSON keys for serialization and deserialization. Property values may be null or empty
    /// if the corresponding address component is not available.</remarks>
    public class Components
    {
        [JsonProperty("primary_number")]
        public long PrimaryNumber { get; set; }

        [JsonProperty("street_name")]
        public string StreetName { get; set; }

        [JsonProperty("street_suffix")]
        public string StreetSuffix { get; set; }

        [JsonProperty("city_name")]
        public string CityName { get; set; }

        [JsonProperty("default_city_name")]
        public string DefaultCityName { get; set; }

        [JsonProperty("state_abbreviation")]
        public string StateAbbreviation { get; set; }

        [JsonProperty("zipcode")]
        public string Zipcode { get; set; }

        [JsonProperty("plus4_code")]
        public string Plus4Code { get; set; }

        [JsonProperty("delivery_point")]
        public string DeliveryPoint { get; set; }

        [JsonProperty("delivery_point_check_digit")]
        public string DeliveryPointCheckDigit { get; set; }
    }

    /// <summary>
    /// Represents supplemental metadata information for a geographic or postal record, including location,
    /// administrative, and delivery details.
    /// </summary>
    /// <remarks>The properties of this class provide access to various attributes such as record type, ZIP
    /// code type, county information, carrier route, congressional district, residential delivery indicator (RDI),
    /// enhanced line of travel (eLOT) data, geographic coordinates, time zone, and daylight saving time status. This
    /// class is typically used to enrich address or location data with additional context for validation, analysis, or
    /// delivery optimization scenarios.</remarks>
    public class Metadata
    {
        [JsonProperty("record_type")]
        public string RecordType { get; set; }

        [JsonProperty("zip_type")]
        public string ZipType { get; set; }

        [JsonProperty("county_fips")]
        public string CountyFips { get; set; }

        [JsonProperty("county_name")]
        public string CountyName { get; set; }

        [JsonProperty("carrier_route")]
        public string CarrierRoute { get; set; }

        [JsonProperty("congressional_district")]
        public string CongressionalDistrict { get; set; }

        [JsonProperty("rdi")]
        public string Rdi { get; set; }

        [JsonProperty("elot_sequence")]
        public string ElotSequence { get; set; }

        [JsonProperty("elot_sort")]
        public string ElotSort { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("precision")]
        public string Precision { get; set; }

        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }

        [JsonProperty("utc_offset")]
        public long UtcOffset { get; set; }

        [JsonProperty("dst")]
        public bool Dst { get; set; }
    }
}