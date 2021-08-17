using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class PostCreateCourseDemandRequest : IPostApiRequest
    {
        private readonly Guid _id;

        public PostCreateCourseDemandRequest (CreateCourseDemandData data)
        {
            Data = data;
            _id = data.Id;
        }

        public string PostUrl => $"api/demand/{_id}";
        public object Data { get; set; }
    }

    public class CreateCourseDemandData
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("organisationName")]
        public string OrganisationName { get; set; }

        [JsonProperty("contactEmailAddress")]
        public string ContactEmailAddress { get; set; }

        [JsonProperty("numberOfApprentices")]
        public int NumberOfApprentices { get; set; }

        [JsonProperty("course")]
        public Course Course { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }
        [JsonProperty("stopSharingUrl")]
        public string StopSharingUrl { get; set; }
        [JsonProperty("startSharingUrl")]
        public string StartSharingUrl { get; set; }
        [JsonProperty("expiredCourseDemandId")]
        public Guid? ExpiredCourseDemandId { get; set; }
        [JsonProperty("entryPoint")]
        public short? EntryPoint { get; set; }
    }
    
    public class Course
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

    public class Location
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("locationPoint")]
        public LocationPoint LocationPoint { get; set; }
    }

    public class LocationPoint
    {
        [JsonProperty("geoPoint")]
        public List<double> GeoPoint { get; set; }
    }
}