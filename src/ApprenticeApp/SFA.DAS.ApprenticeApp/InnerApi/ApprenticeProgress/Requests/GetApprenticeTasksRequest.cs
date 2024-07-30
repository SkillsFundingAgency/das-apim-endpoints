using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class GetApprenticeTasksRequest : IGetApiRequest
    {
        public Guid ApprenticeshipId;
        public int Status;
        public string FromDate;
        public string ToDate;
        

        public GetApprenticeTasksRequest(Guid apprenticeshipId, int status, string fromDate, string toDate)
        {
            ApprenticeshipId = apprenticeshipId;
            Status = status;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public string GetUrl => $"apprenticeships/{ApprenticeshipId}/fromDate/{FromDate}/toDate/{ToDate}/status/{Status}";
    }
}
