namespace SFA.DAS.Apprenticeships.Api.Models;
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

public class CreateApprenticeshipStartDateChangeRequest
{
    public string Initiator { get; set; }
    public string UserId { get; set; }
    public DateTime ActualStartDate { get; set; }
	public DateTime PlannedEndDate { get; set; }
    public string Reason { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.