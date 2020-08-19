using MediatR;
using System;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApplicationAccountLegalEntity
{
    public class GetApplicationAccountLegalEntityQuery : IRequest<long>
    {
        public long AccountId { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
