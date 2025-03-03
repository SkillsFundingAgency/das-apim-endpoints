using System.Collections.Generic;
using SFA.DAS.EmployerAccounts.Application.Models;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAgreementTemplates;
public class GetEmployerAgreementTemplatesResponse
{
    public List<EmployerAgreementTemplate> EmployerAgreementTemplates { get; set; }
}
