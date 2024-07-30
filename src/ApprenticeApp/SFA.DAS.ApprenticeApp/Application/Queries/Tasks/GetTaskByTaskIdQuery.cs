using System;
using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetTaskByTaskIdQuery : IRequest<GetTaskByTaskIdQueryResult>
    {
        public Guid ApprenticeshipId { get; set; }
        public int TaskId { get; set; }
    }
}