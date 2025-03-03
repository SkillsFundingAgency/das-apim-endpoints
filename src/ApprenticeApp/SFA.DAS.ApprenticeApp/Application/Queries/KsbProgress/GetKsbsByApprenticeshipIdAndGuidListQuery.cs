using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress
{
    public class GetKsbsByApprenticeshipIdAndGuidListQuery : IRequest<GetKsbsByApprenticeshipIdAndGuidListQueryResult>
    {
        public long ApprenticeshipId { get; set; }
        public Guid[] Guids { get; set; }
    }
}
