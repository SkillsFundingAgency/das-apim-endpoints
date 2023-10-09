using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.ProviderAccounts.Queries
{
    public class GetRoatpV2ProviderQuery : IRequest<bool>
    {
        public int Ukprn { get; set; }
    }
}