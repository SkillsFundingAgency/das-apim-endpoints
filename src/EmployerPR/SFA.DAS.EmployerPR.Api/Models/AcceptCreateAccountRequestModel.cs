namespace SFA.DAS.EmployerPR.Api.Models;

public record AcceptCreateAccountRequestModel(string FirstName, string LastName, string Email, Guid UserRef);
