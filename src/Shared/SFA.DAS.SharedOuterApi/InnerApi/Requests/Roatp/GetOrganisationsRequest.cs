using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
public class GetOrganisationsRequest : IGetApiRequest
{
    public string GetUrl => ("organisations");
}