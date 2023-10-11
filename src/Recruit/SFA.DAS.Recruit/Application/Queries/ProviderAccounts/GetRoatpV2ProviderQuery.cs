using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.ProviderAccounts
{
    public class GetRoatpV2ProviderQuery : IRequest<bool>
    {
        public int Ukprn { get; set; }
    }
}