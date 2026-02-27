namespace SFA.DAS.SharedOuterApi.Domain.Recruit;

public class ReviewFieldIndicator
{
    public required string FieldIdentifier { get; set; }
    public bool IsChangeRequested { get; set; }
}