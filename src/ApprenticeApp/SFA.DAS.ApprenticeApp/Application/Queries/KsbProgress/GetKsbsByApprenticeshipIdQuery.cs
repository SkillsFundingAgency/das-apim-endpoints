using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress
{
    public class GetKsbsByApprenticeshipIdQuery : IRequest<GetKsbsByApprenticeshipIdQueryResult>
    {
        public Guid ApprenticeshipId { get; set; }
    }
}
