using Newtonsoft.Json;

namespace CoreZipCode.Services.ZipCode.SmartyApi
{
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