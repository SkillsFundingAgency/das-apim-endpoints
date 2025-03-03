using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class GetApprenticeTaskRemindersRequest : IGetApiRequest
    {
        public long ApprenticeshipId;

        public GetApprenticeTaskRemindersRequest(long apprenticeshipId)
        {
            ApprenticeshipId = apprenticeshipId;
        }

        public string GetUrl => $"/apprenticeships/{ApprenticeshipId}/taskreminders";
    }
}
