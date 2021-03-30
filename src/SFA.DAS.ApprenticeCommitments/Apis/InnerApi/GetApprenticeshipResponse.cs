namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class ApprenticeshipResponse
    {
        public long Id { get; set; }
        public string EmployerName { get; set; }
        public string TrainingProviderName { get; set; }
        public bool? TrainingProviderCorrect { get; set; }
        public bool? EmployerCorrect { get; set; }
        public bool? ApprenticeshipDetailsConfirmation { get; set; }
        public bool? ApprenticeshipDeliveryConfirmation { get; set; }
        public bool? RolesAndResponsibilitiesConfirmation { get; set; }
    }
}
