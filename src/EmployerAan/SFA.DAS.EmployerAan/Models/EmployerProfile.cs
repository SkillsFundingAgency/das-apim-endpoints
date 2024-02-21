namespace SFA.DAS.EmployerAan.Models;

public class EmployerProfile
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string GovIdentifier { get; set; }
    public Guid? CorrelationId { get; set; }
}
