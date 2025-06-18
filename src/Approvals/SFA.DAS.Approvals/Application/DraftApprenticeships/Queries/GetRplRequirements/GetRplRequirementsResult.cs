namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetRplRequirements
{
    public class GetRplRequirementsResult
    {
        public string ApprenticeshipType { get; set; }
        public bool IsRequired { get; set; }
        public int? OffTheJobTrainingMinimumHours { get; set; }
    }
} 