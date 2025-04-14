using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;

public class GetAllApprenticeshipsRequest(string ukprn, int academicYear, int page, int? pageSize) : IGetApiRequest
{
    public string GetUrl => $"/{ukprn}/academicyears/{academicYear}/apprenticeships?page={page}&pageSize={pageSize}";
}

