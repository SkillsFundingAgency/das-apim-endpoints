using MediatR;

namespace SFA.DAS.VacanciesManage.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer
{
    public class GetLegalEntitiesForEmployerQuery : IRequest<GetLegalEntitiesForEmployerResult>
    {
        public string EncodedAccountId { get; set; }
    }
}