﻿using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.Models
{
    public class SendResponseNotificationEmailParameters
    {
        public Guid RequestedBy { get; set; }
        public long AccountId { get; set; }
        public List<StandardDetails> Standards { get; set; }
        public string ManageNotificationSettingsLink { get; set; }
        public string ManageRequestsLink { get; set; }
    }
    public class StandardDetails
    {
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
    }
}
