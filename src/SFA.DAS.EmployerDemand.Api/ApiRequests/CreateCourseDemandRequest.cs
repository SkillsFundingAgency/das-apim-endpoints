using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerDemand.Api.ApiRequests
{
    public class CreateCourseDemandRequest
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("OrganisationName")]
        public string OrganisationName { get; set; }

        [JsonProperty("NumberOfApprentices")]
        public int NumberOfApprentices { get; set; }

        [JsonProperty("ContactEmailAddress")]
        public string ContactEmailAddress { get; set; }

        [JsonProperty("LocationItem")]
        public LocationItem LocationItem { get; set; }

        [JsonProperty("TrainingCourse")]
        public TrainingCourse TrainingCourse { get; set; }
        [JsonProperty("ResponseUrl")]
        public string ResponseUrl { get; set; }
        [JsonProperty("StopSharingUrl")]
        public string StopSharingUrl { get; set; }
        [JsonProperty("StartSharingUrl")]
        public string StartSharingUrl { get ; set ; }
        [JsonProperty("ExpiredCourseDemandId")]
        public Guid? ExpiredCourseDemandId { get ; set ; }
        [JsonProperty("EntryPoint")]
        public short? EntryPoint { get ; set ; }
    }

    public class LocationItem
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Location
    {
        [JsonProperty("geoPoint")]
        public List<double> GeoPoint { get; set; }
    }

    public class TrainingCourse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("route")]
        public string Route { get ; set ; }
    }
}