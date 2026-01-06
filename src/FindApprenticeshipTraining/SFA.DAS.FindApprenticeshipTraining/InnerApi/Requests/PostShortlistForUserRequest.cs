using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class PostShortlistForUserRequest : IPostApiRequest
    {
        public string PostUrl => "shortlists";
        public object Data { get; set; }
    }

    public class PostShortlistData
    {
        public Guid UserId { get; set; }
        public int Ukprn { get; set; }
        public string LarsCode { get; set; }
        public string LocationDescription { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }

    public class PostShortListResponse
    {
        public Guid ShortlistId { get; set; }
        public bool IsCreated { get; set; }
    }
}