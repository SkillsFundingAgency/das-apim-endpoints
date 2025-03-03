using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;

public record PostCreateAccountRequest : IPostApiRequest<CreateAccountRequestBody>
{
    public string PostUrl => "api/accounts";

    public CreateAccountRequestBody Data { get; set; }
}

public class CreateAccountRequestBody
{
    public string RequestId { get; set; }
    public Guid UserRef { get; set; }
    public string Email { get; set; }
    public string EmployerOrganisationName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmployerPaye { get; set; }
    public string EmployerAorn { get; set; }
    public string EmployerAddress { get; set; }
    public string EmployerOrganisationReferenceNumber { get; set; }
}
