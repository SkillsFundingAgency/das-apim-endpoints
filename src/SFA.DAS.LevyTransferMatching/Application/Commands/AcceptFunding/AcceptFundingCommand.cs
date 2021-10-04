using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding
{
    public class AcceptFundingCommand : IRequest<bool>
    {
        public long AccountId { get; set; }
        public int ApplicationId { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
    }
}
