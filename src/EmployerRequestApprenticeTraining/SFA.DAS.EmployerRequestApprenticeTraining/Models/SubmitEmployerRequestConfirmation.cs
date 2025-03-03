namespace SFA.DAS.EmployerRequestApprenticeTraining.Models
{
    public class SubmitEmployerRequestConfirmation : EmployerRequestConfirmation
    {
        public int ExpiryAfterMonths { get; set; }
        public string RequestedByEmail { get; set; }
    }
}
