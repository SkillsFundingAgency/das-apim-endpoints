using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference
{
    public class GetApprenticeshipVacancyQuery : IRequest<GetApprenticeshipVacancyQueryResult>
    {
        [Required]
        public string VacancyReference { get; set; }
    }
}
