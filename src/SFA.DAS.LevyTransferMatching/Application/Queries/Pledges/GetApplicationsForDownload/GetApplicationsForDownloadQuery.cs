using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationsForDownload
{
    public class GetApplicationsForDownloadQuery : IRequest<GetApplicationsForDownloadQueryResult>
    {
        public int PledgeId { get; set; }
        public long AccountId { get; set; }
    }
}
