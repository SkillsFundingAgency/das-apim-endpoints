﻿using MediatR;
using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeCommand : IRequest<CreatePledgeResult>
    {
        public Pledge Pledge { get; set; }
    }
}