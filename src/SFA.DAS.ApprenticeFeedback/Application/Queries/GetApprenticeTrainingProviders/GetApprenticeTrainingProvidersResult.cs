using SFA.DAS.ApprenticeFeedback.Models;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeTrainingProviders
{
    public class GetApprenticeTrainingProvidersResult
    {
        public int RecentDenyPeriodDays { get; set; }
        public int InitialDenyPeriodDays { get; set; }
        public int FinalAllowedPeriodDays { get; set; }
        public int MinimumActiveApprenticeshipCount { get; set; }
        public IEnumerable<TrainingProvider> TrainingProviders { get; set; }
    }
}
