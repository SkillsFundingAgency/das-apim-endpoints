using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApplicationLegalEntity
{
    public class GetApplicationLegalEntityQuery : IRequest<long>
    {
        public long AccountId { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
