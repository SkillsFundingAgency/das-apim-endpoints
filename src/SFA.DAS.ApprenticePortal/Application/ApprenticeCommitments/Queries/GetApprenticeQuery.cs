using MediatR;
using System;

namespace SFA.DAS.ApprenticePortal.Application.ApprenticeCommitments.Queries
{
    public class GetApprenticeQuery : IRequest<GetApprenticeQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}
