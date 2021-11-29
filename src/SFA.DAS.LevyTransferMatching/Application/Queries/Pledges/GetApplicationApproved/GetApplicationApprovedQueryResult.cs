using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationApproved
{
    public class GetApplicationApprovedQueryResult
    {
        public string EmployerAccountName { get; set; }
        public bool AutomaticApproval { get; set; }
    }
}
