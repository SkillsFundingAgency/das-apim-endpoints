using System.Collections.Generic;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Manage.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer
{
    public class GetLegalEntitiesForEmployerResult
    {
        public IEnumerable<GetEmployerAccountLegalEntityItem> LegalEntities { get; set; }
    }
}