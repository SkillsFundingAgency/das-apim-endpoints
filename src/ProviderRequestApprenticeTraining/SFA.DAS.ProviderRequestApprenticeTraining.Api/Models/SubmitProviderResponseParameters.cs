using System.Collections.Generic;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Models
{
    public class SubmitProviderResponseParameters
    {
        public List<Guid> EmployerRequestIds { get; set; } = new List<Guid>();
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string CurrentUserEmail { get; set; }
        public string ContactName { get; set; }
        public Guid RespondedBy { get; set; }
    }
}
