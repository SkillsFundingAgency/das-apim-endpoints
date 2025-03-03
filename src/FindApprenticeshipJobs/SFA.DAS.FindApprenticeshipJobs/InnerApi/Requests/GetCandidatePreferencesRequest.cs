using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

public class GetCandidatePreferencesRequest : IGetApiRequest
{
    public string GetUrl => "api/referencedata/preferences";
}