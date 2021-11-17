using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi
{
    public class Confirmations
    {
        public bool? EmployerCorrect { get; set; }
        public bool? TrainingProviderCorrect { get; set; }
        public bool? ApprenticeshipDetailsCorrect { get; set; }
        public RolesAndResponsibilitiesConfirmations RolesAndResponsibilitiesConfirmations { get; set; }
        public bool? HowApprenticeshipDeliveredCorrect { get; set; }
        public bool? ApprenticeshipCorrect { get; set; }
    }

    [Flags]
    public enum RolesAndResponsibilitiesConfirmations
    {
        NoneConfirmed = 0,
        ApprenticeRolesAndResponsibilitiesConfirmed = 1,
        EmployerRolesAndResponsibilitiesConfirmed = 2,
        ProviderRolesAndResponsibilitiesConfirmed = 4
    }
}
