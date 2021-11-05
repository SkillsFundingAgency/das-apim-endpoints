using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationsForDownload;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetApplicationsForDownloadResponse
    {
        public IEnumerable<ApplicationForDownloadModel> Applications { get; set; }
    }
}
