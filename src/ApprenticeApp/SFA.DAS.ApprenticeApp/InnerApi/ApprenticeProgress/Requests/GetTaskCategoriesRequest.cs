using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class GetTaskCategoriesRequest : IGetApiRequest
    {
        public long ApprenticeshipId;     

        public GetTaskCategoriesRequest(long apprenticeshipId)
        {
            ApprenticeshipId = apprenticeshipId;
        }

        public string GetUrl => $"apprenticeships/{ApprenticeshipId}/taskCategories";
    }
}
