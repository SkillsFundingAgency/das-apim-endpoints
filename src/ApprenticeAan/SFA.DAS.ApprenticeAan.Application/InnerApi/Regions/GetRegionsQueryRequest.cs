using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Regions;

public class GetRegionsQueryRequest : IGetApiRequest
{
    public string GetUrl => Constants.AanHubApiUrls.GetRegionsUrl;
}