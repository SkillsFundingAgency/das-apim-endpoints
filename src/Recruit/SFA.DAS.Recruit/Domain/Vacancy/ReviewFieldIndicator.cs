namespace SFA.DAS.Recruit.Domain.Vacancy;

public class ReviewFieldIndicator
{
    public required string FieldIdentifier { get; set; }
    public bool IsChangeRequested { get; set; }
}