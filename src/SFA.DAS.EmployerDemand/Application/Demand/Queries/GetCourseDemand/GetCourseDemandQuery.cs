using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCourseDemand
{
    public class GetCourseDemandQuery : IRequest<GetCourseDemandQueryResult>
    {
        public Guid Id { get; set; }
    }
}