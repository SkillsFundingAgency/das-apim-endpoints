using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
public class GetUserByEmailResponse
{
    public long Id { get; set; }
    public Guid? Ref { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CorrelationId { get; set; }
    public DateTime? TermAndConditionsAcceptedOn { get; set; }
    public string FullName { get; set; }
}
