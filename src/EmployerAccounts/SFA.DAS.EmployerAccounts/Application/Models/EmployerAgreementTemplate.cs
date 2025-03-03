using System;
using SFA.DAS.Common.Domain.Types;

namespace SFA.DAS.EmployerAccounts.Application.Models;
public class EmployerAgreementTemplate
{
    public int Id { get; set; }
    public string PartialViewName { get; set; }
    public DateTime CreatedDate { get; set; }
    public int VersionNumber { get; set; }
    public AgreementType AgreementType { get; set; }
    public DateTime? PublishedDate { get; set; }
}
