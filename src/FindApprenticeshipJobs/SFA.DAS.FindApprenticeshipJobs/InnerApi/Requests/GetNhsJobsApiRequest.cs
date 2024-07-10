using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

public class GetNhsJobsApiRequest(int pageNumber) : IGetApiRequest
{
    public string GetUrl => $"?contractType=Apprenticeship&page={pageNumber}";
}