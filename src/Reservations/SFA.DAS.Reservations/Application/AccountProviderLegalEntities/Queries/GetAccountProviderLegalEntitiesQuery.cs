using System.Collections.Generic;
using MediatR;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.Reservations.Application.AccountProviderLegalEntities.Queries
{
    public class GetAccountProviderLegalEntitiesQuery : IRequest<GetAccountProviderLegalEntitiesResult>
    {
        public int? Ukprn { get; set; }
        public List<Operation> Operations { get; set; }
    }
}
