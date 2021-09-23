using System.Collections.Generic;
using Microsoft.Azure.Amqp.Serialization;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications
{
    public class GetApplicationsResult
    {
        public List<Models.Application> Applications { get; set; }
    }
}