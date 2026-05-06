using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
public record GetCivilServiceJobsApiRequest : IGetApiRequest
{
    public string GetUrl => "/civilServiceVacancies";
}