using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.VacanciesManage.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer;

public class GetLegalEntitiesForEmployerResult
{
    public IEnumerable<GetEmployerAccountLegalEntityItem> LegalEntities { get; set; }
}