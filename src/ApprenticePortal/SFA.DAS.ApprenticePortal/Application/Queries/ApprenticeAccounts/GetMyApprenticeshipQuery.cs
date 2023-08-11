using MediatR;
using System;

namespace SFA.DAS.ApprenticePortal.Application.Queries.ApprenticeAccounts
{
    public class GetMyApprenticeshipQuery : IRequest<GetMyApprenticeshipQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}