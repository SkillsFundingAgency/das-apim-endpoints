using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class GetQualificationsReferenceDataApiRequest: IGetApiRequest
    {
        public string GetUrl => $"api/referencedata/qualifications";
    }
}
