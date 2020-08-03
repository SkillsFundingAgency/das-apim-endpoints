using System;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class ConfirmApplicationRequest
    {
        public Guid ApplicationId { get; }
        public long AccountId { get; }
        public DateTime DateSubmitted { get; set; }
    }
}
