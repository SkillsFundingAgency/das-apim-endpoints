namespace SFA.DAS.ApprenticeApp.Models
{
    public class ApprenticeDetails
    {
        public Apprentice Apprentice { get; set; }
        public ApprenticeshipsList? Apprenticeship { get; set; }
        public MyApprenticeship MyApprenticeship { get; set; }
    }
}
