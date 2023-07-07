using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions
{
    public class ApplicationsWithAutomaticApprovalQuery : IRequest<ApplicationsWithAutomaticApprovalQueryResult>
    {
        public int? PledgeId { get; set; }
    }

}
