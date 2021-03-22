using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class PostCreateCourseDemandRequest : IPostApiRequest<CreateCourseDemandRequest>
    {
        public PostCreateCourseDemandRequest (CreateCourseDemandRequest data)
        {
            Data = data;
        }

        public string PostUrl => "api/demand/create";
        public CreateCourseDemandRequest Data { get; set; }
    }

    public class CreateCourseDemandRequest
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("organisationName")]
        public string OrganisationName { get; set; }

        [JsonProperty("contactEmailAddress")]
        public string ContactEmailAddress { get; set; }

        [JsonProperty("numberOfApprentices")]
        public long NumberOfApprentices { get; set; }

        [JsonProperty("course")]
        public Course Course { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }
    }
    
    public class Course
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }
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
        public List<long> GeoPoint { get; set; }
    }
}