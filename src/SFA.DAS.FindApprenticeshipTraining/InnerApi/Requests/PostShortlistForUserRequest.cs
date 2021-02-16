using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class PostShortlistForUserRequest : IPostApiRequest
    {
        public string PostUrl => "api/shortlist";
        public object Data { get; set; }
    }

    public class PostShortlistData
    {
        public Guid ShortlistUserId { get ; set ; }
        public float? Lat { get ; set ; }
        public float? Lon { get ; set ; }
        public int StandardId { get ; set ; }
        public string LocationDescription { get ; set ; }
        public int Ukprn { get ; set ; }
        public string SectorSubjectArea { get ; set ; }
    }

    public class PostShortListResponse
    {
        public Guid Id { get ; set ; }
    }
}