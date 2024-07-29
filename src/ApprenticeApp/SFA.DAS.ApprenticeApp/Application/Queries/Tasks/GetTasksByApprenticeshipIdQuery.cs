using System;
using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetTasksByApprenticeshipIdQuery : IRequest<GetTasksByApprenticeshipIdQueryResult>
    {
        public Guid ApprenticeshipId { get; set; }
        public int Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}