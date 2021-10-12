using MediatR;

namespace SFA.DAS.Vacancies.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer
{
    public class GetLegalEntitiesForEmployerQuery : IRequest<GetLegalEntitiesForEmployerResult>
    {
        public string EncodedAccountId { get; set; }
    }
}