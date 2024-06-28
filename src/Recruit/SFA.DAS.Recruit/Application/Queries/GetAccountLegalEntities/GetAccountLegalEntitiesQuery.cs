using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetAccountLegalEntities
{
    public class GetAccountLegalEntitiesQuery : IRequest<GetAccountLegalEntitiesQueryResult>
    {
        public long AccountId { get; set; }
    }
}