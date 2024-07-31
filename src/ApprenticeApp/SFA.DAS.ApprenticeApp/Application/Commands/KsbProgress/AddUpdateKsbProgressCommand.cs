﻿using System;
using MediatR;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class AddUpdateKsbProgressCommand : IRequest<Unit>
    {
        public Guid ApprenticeshipId { get; set; }
        public ApprenticeKsbProgressData Data { get; set; }
    }
}
