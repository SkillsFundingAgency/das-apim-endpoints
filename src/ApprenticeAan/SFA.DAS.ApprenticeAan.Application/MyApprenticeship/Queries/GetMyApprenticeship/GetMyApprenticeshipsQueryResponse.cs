namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship;

public class GetMyApprenticeshipsQueryResponse
{
    public Guid ApprenticeId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }

    public IEnumerable<MyApprenticeshipResponse>? MyApprenticeships { get; set; }
}