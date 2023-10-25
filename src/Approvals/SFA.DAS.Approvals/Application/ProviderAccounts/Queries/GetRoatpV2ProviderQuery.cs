using MediatR;

namespace SFA.DAS.Approvals.Application.ProviderAccounts.Queries
{
    public class GetRoatpV2ProviderQuery : IRequest<bool>
    {
        public int Ukprn { get; set; }
    }
}