using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress
{
    public class GetKsbProgressForTaskQuery : IRequest<GetKsbProgressForTaskQueryResult>
    {
        public long ApprenticeshipId { get; set; }
        public int TaskId { get; set; }
    }
}
