using MediatR;
using System;

namespace SFA.DAS.ApprenticePortal.Application.ApprenticeAccounts.Queries
{
    public class GetMyApprenticeshipQuery : IRequest<GetMyApprenticeshipQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}