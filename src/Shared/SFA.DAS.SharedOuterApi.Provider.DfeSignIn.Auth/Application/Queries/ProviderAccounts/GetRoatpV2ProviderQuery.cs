using MediatR;

namespace SFA.DAS.SharedOuterApi.Provider.DfeSignIn.Auth.Application.Queries.ProviderAccounts
{
    public class GetRoatpV2ProviderQuery : IRequest<bool>
    {
        public int Ukprn { get; set; }
    }
}