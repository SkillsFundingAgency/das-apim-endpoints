using System;
using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries
{
    public class GetApprenticeQuery : IRequest<GetApprenticeResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}