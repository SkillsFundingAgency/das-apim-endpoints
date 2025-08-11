using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Responses;

public class GetCourseProvidersApiResponse
{
    public int TotalPages { get; init; }
    public List<ProviderData> Providers { get; init; }
}

public class ProviderData
{
    public int Ukprn { get; set; }
    public string ProviderName { get; set; }
}