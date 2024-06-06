namespace SFA.DAS.Apprenticeships.Api.Models;

public class CreateApprenticeshipStartDateChangeRequest
{
    public string Initiator { get; set; }
    public string UserId { get; set; }
    public DateTime ActualStartDate { get; set; }
    public string Reason { get; set; }
}