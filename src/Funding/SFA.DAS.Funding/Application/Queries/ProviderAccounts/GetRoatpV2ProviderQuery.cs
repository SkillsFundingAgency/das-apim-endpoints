using MediatR;

namespace SFA.DAS.Funding.Application.Queries.ProviderAccounts
{
    public class GetRoatpV2ProviderQuery : IRequest<bool>
    {
        public int Ukprn { get; set; }
    }
}