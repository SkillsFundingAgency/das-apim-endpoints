using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index
{
    public class GetIndexQuery : IRequest<GetIndexQueryResult>
    {
        public string ApplicantEmailAddress { get; set; }
        public string VacancyReference { get; set; }
    }
}
