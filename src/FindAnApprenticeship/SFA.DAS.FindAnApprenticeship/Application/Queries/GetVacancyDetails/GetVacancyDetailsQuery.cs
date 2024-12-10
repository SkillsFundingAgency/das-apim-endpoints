using System.ComponentModel.DataAnnotations;
using MediatR;


namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetVacancyDetails
{
    public record GetVacancyDetailsQuery([Required] string VacancyReference) : IRequest<GetVacancyDetailsQueryResult>;
}