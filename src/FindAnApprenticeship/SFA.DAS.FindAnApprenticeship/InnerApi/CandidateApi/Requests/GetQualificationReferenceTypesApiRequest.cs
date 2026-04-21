using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public class GetQualificationReferenceTypesApiRequest : IGetApiRequest
{
    public string GetUrl => "api/referencedata/qualifications";
}