using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class GetKsbsByApprenticeshipIdAndGuidListRequest : IGetApiRequest
    {
        public Guid ApprenticeshipId;
        public string Ksbs;

        public GetKsbsByApprenticeshipIdAndGuidListRequest(Guid apprenticeshipId, Guid[] ksbGuids)
        {
            ApprenticeshipId = apprenticeshipId;
            foreach (var ksb in ksbGuids)
            {
                Ksbs += ksb.ToString() + "&guids=";
            }
            Ksbs = Ksbs.Substring(0, Ksbs.Length - 7);
        }

        public string GetUrl => $"apprenticeships/{ApprenticeshipId}/ksbs?guids={Ksbs}";
    }
}
