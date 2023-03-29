using MediatR;
using System;

namespace SFA.DAS.ApprenticePortal.Application.Queries.Homepage
{
    public class GetApprenticeHomepageQuery : IRequest<GetApprenticeHomepageQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}
