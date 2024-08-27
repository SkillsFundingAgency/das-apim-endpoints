using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using static SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining.SubmitEmployerRequestRequest;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    public class SubmitEmployerRequestRequest : IPostApiRequest<SubmitEmployerRequestData>
    {
        public long AccountId { get; set; }

        public SubmitEmployerRequestData Data { get; set; }

        public SubmitEmployerRequestRequest(long accountId, SubmitEmployerRequestData data)
        {
            AccountId = accountId;
            Data = data;
        }

        public string PostUrl => $"api/employerrequest/account/{AccountId}/submit-request";

        public class SubmitEmployerRequestData
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
