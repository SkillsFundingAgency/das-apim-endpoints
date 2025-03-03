using System;
using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetTaskCategoriesQuery : IRequest<GetTaskCategoriesQueryResult>
    {
        public long ApprenticeshipId { get; set; }
    }
}