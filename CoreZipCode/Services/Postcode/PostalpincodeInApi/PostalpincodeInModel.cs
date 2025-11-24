using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CoreZipCode.Services.Postcode.PostalpincodeInApi
{
    /// <summary>
    /// Represents the response model for postal pincode queries, containing status information, a message, and a
    /// collection of post office details.  
    /// </summary>
    /// <remarks>This class is typically used to deserialize responses from postal pincode APIs. The
    /// properties correspond to the expected fields in the API response, allowing access to the status, message, and
    /// associated post office data.</remarks>
    [Serializable]
    public class PostalpincodeInModel
    {
        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("PostOffice")]
        public List<PostOffice> PostOffice { get; set; }
    }

    /// <summary>
    /// Represents a postal office location and its associated details, including name, branch type, delivery status,
    /// and geographic information.
    /// </summary>
    /// <remarks>This class is typically used to model information about a post office for address validation,
    /// postal lookup, or geographic categorization scenarios. All properties are read-write and correspond to standard
    /// attributes found in postal systems. Instances of this class can be serialized for data exchange or storage
    /// purposes.</remarks>
    [Serializable]
    public class PostOffice
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("BranchType")]
        public string BranchType { get; set; }

        [JsonProperty("DeliveryStatus")]
        public string DeliveryStatus { get; set; }

        [JsonProperty("Taluk")]
        public string Taluk { get; set; }

        [JsonProperty("Circle")]
        public string Circle { get; set; }

        [JsonProperty("District")]
        public string District { get; set; }

        [JsonProperty("Division")]
        public string Division { get; set; }

        [JsonProperty("Region")]
        public string Region { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }
    }
}
