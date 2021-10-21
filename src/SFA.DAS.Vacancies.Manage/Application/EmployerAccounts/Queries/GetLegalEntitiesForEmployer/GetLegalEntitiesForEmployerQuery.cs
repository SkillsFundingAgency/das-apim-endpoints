using MediatR;

namespace SFA.DAS.Vacancies.Manage.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer
{
    public class GetLegalEntitiesForEmployerQuery : IRequest<GetLegalEntitiesForEmployerResult>
    {
        public string EncodedAccountId { get; set; }
    }
}