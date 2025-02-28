using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class PostShortlistForUserRequest : IPostApiRequest
    {
        public string PostUrl => "api/shortlists";
        public object Data { get; set; }
    }

    public class PostShortlistData
    {
        public Guid UserId { get; set; }
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string LocationDescription { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
    }

    public class PostShortListResponse
    {
        public Guid ShortlistId { get; set; }
    }
}