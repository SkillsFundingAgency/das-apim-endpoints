using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using static SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests.PostSubmitEmployerRequestRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests
{
    public class PostSubmitEmployerRequestRequest : IPostApiRequest<PostSubmitEmployerRequestData>
    {
        public long AccountId { get; set; }

        public PostSubmitEmployerRequestData Data { get; set; }

        public PostSubmitEmployerRequestRequest(long accountId, PostSubmitEmployerRequestData data)
        {
            AccountId = accountId;
            Data = data;
        }

        public string PostUrl => $"api/accounts/{AccountId}/employer-requests";

        public class PostSubmitEmployerRequestData
        {
            public string OriginalLocation { get; set; }
            public RequestType RequestType { get; set; }
            public string StandardReference { get; set; }
            public int NumberOfApprentices { get; set; }
            public string SameLocation { get; set; }
            public string SingleLocation { get; set; }
            public double SingleLocationLatitude { get; set; }
            public double SingleLocationLongitude { get; set; }
            public string[] MultipleLocations { get; set; }
            public bool AtApprenticesWorkplace { get; set; }
            public bool DayRelease { get; set; }
            public bool BlockRelease { get; set; }
            public Guid RequestedBy { get; set; }
            public Guid ModifiedBy { get; set; }
        }
    }
}
