using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeApp.Models
{
    [ExcludeFromCodeCoverage]
    public class Confirmations
    {
        public bool? EmployerCorrect { get; set; }
        public bool? TrainingProviderCorrect { get; set; }
        public bool? ApprenticeshipDetailsCorrect { get; set; }
        public RolesAndResponsibilitiesConfirmations? RolesAndResponsibilitiesConfirmations { get; set; }
        public bool? HowApprenticeshipDeliveredCorrect { get; set; }
        public bool? ApprenticeshipCorrect { get; set; }
    }
}
