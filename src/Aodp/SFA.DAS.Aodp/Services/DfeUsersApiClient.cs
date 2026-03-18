using SFA.DAS.Aodp.Configuration;
//using SFA.DAS.SharedOuterApi.Infrastructure.Authentication;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Services;

public interface IDfeUsersApiClient<T> : IGetApiClient<T>
{
}

public class DfeUsersApiClient
    : DfeSignInApiClient<DfeSignInApiConfiguration>,
        IDfeUsersApiClient<DfeSignInApiConfiguration>
{
    public DfeUsersApiClient(
        IHttpClientFactory httpClientFactory,
        DfeSignInApiConfiguration apiConfiguration,
        IDfeJwtProvider jwtProvider)
        : base(httpClientFactory, apiConfiguration, jwtProvider)
    {
    }
}