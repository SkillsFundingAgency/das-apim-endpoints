using MediatR;
using System;

namespace SFA.DAS.ApprenticePortal.Application.Queries.ApprenticeAccounts
{
    public class GetApprenticeQuery : IRequest<GetApprenticeQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}