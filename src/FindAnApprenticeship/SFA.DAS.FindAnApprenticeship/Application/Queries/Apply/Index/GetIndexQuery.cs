using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index
{
    public class GetIndexQuery : IRequest<GetIndexQueryResult>
    {
        public Guid CandidateId { get; set; }
        public string VacancyReference { get; set; }
    }
}
