using MediatR;
using System;

namespace SFA.DAS.ApprenticePortal.Application.ApprenticeAccounts.Queries
{
    public class GetCurrentApprenticeshipQuery : IRequest<GetCurrentApprenticeshipQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}