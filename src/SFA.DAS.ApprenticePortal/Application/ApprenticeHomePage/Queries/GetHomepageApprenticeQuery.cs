using MediatR;
using System;

namespace SFA.DAS.ApprenticePortal.Application.ApprenticeHomePage.Queries
{
    public class GetHomepageApprenticeQuery : IRequest<GetHomepageApprenticeQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}
