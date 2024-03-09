using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Homepage
{
    public class GetApprenticeHomepageQuery : IRequest<GetApprenticeHomepageQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}
