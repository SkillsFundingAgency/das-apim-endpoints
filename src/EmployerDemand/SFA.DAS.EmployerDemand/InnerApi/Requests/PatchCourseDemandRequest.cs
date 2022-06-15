using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class PatchCourseDemandRequest : IPatchApiRequest<List<PatchOperation>>
    {
        private readonly Guid _id;

        public PatchCourseDemandRequest(Guid id, PatchOperation data)
        {
            _id = id;
            Data = new List<PatchOperation> {data};
        }

        public string PatchUrl => $"api/demand/{_id}";
        public List<PatchOperation> Data { get; set; }
    }
    
    public class PatchOperation
    {
        [JsonProperty("value")]
        public object Value { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("op")]
        public string Op => "replace";
    }
}