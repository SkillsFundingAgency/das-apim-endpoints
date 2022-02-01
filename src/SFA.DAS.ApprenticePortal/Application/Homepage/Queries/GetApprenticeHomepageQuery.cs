using MediatR;
using System;

namespace SFA.DAS.ApprenticePortal.Application.Homepage.Queries
{
    public class GetApprenticeHomepageQuery : IRequest<GetApprenticeHomepageQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}
