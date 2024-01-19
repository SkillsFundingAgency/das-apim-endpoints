using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command
{
    public class ValidateChangeOfEmployerOverlapRequest
    {
        public string Uln { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long ProviderId { get; set; }
    }
}
