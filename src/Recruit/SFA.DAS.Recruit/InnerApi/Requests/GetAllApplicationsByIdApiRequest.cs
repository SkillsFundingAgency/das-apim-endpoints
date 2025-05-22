using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public record GetAllApplicationsByIdApiRequest(GetAllApplicationsByIdApiRequestData Payload) : IPostApiRequest
    {
        public string PostUrl => "api/applications/getAll";
        public object Data { get; set; } = Payload;
    }

    public record GetAllApplicationsByIdApiRequestData
    {
        public IEnumerable<Guid> ApplicationIds { get; set; }
        public bool IncludeDetails { get; set; } = false;
    }
}