﻿using System;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class ConfirmApplicationRequest
    {
        public Guid ApplicationId { get; set; }
        public long AccountId { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string SubmittedBy { get; set; }
    }
}
