﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge
{ 
    public class ClosePledgeCommand : IRequest<ClosePledgeCommandResult>
    {
        public int PledgeId { get; set; }
    }
}
