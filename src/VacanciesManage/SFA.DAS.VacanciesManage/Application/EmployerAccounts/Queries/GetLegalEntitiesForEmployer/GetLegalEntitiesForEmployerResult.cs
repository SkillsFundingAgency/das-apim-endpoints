using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.VacanciesManage.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer
{
    public class GetLegalEntitiesForEmployerResult
    {
        public IEnumerable<GetEmployerAccountLegalEntityItem> LegalEntities { get; set; }
    }
}