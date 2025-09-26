namespace SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetProviderSummary;

public sealed class ReviewsModel
{
    public string ReviewPeriod { get; set; }
    public string EmployerReviews { get; set; }
    public string EmployerStars { get; set; }
    public string EmployerRating { get; set; }
    public string ApprenticeReviews { get; set; }
    public string ApprenticeStars { get; set; }
    public string ApprenticeRating { get; set; }
}