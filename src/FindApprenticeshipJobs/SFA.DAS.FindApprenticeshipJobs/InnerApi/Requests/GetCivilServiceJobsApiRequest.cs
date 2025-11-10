using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
public record GetCivilServiceJobsApiRequest : IGetApiRequest
{
    public string GetUrl => "/civilServiceVacancies";
}