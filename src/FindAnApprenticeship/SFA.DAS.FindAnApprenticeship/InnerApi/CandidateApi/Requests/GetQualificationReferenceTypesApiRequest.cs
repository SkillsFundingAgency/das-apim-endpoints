using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public class GetQualificationReferenceTypesApiRequest : IGetApiRequest
{
    public string GetUrl => "api/referencedata/qualifications";
}