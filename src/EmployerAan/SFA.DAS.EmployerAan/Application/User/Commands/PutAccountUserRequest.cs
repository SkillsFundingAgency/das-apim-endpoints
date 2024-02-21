namespace SFA.DAS.EmployerAan.Application.User.Commands;
public class PutAccountUserRequest
{
    public string UserRef { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Guid? CorrelationId { get; set; }

}
