using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Homepage
{
    public class GetApprenticeDetailsQuery : IRequest<GetApprenticeDetailsQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}
