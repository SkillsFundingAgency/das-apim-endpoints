using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice
{
    public class GetApprenticeQuery : IRequest<GetApprenticeResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}
