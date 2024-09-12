using System;
using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetTaskByTaskIdQuery : IRequest<GetTaskByTaskIdQueryResult>
    {
        public long ApprenticeshipId { get; set; }
        public int TaskId { get; set; }
    }
}