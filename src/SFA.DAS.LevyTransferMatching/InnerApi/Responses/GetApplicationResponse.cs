﻿using System;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Responses
{
    public class GetApplicationResponse
    {
        public string Postcode { get; set; }
        public string StandardId { get; set; }
        public IEnumerable<string> Sectors { get; set; }
        public int NumberOfApprentices { get; set; }
        public DateTime StartDate { get; set; }
        public bool HasTrainingProvider { get; set; }
        public string Details { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public string BusinessWebsite { get; set; }
        public string EmployerAccountName { get; set; }
    }
}