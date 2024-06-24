using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.Models
{
    public class SubmitEmployerRequestConfirmation
    {
        public Guid EmployerRequestId { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public int NumberOfApprentices { get; set; }
        public string SingleLocation { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public string RequestedByEmail { get; set; }
    }
}
