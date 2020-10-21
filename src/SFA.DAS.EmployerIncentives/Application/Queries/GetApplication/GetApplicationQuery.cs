using System;
using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApplication
{
    public class GetApplicationQuery : IRequest<GetApplicationResult>
    {
        public long AccountId { get ; set ; }
        
        public Guid ApplicationId { get; set; }

        public bool IncludeApprenticeships { get; set; }
    }
}