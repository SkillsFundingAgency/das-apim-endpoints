namespace SFA.DAS.EmployerAan.Application.User.Commands;

public class PutEmployerProfileRequest
{
    public string? GovIdentifier { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
};

