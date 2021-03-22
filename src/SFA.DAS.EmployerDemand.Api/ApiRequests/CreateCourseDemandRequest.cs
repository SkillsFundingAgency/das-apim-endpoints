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
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Level")]
        public int Level { get; set; }
    }
}