using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetApprenticeDetailsQuery : IRequest<GetApprenticeDetailsQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}