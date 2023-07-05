using MediatR;
using SFA.DAS.LevyTransferMatching.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApproveAutomaticApplication
{
    public class ApproveAutomaticApplicationCommand : IRequest
    {
        public int ApplicationId { get; set; }
        public int PledgeId { get; set; }
       
    }
}
