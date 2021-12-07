using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Manage.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer
{
    public class GetLegalEntitiesForEmployerResult
    {
        public IEnumerable<GetEmployerAccountLegalEntityItem> LegalEntities { get; set; }
    }
}