using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Configuration
{
    [ExcludeFromCodeCoverage]
    public class EmployerRequestApprenticeTrainingConfiguration
    {
        public List<NotificationTemplate> NotificationTemplates { get; set; }
        public string EmployerRequestApprenticeshipTrainingWebBaseUrl { get; set; }
    }
}
