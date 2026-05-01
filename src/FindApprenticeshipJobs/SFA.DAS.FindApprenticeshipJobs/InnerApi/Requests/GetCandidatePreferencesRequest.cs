using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

public class GetCandidatePreferencesRequest : IGetApiRequest
{
    public string GetUrl => "api/referencedata/preferences";
}