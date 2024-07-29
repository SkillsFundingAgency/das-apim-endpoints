using System;
using MediatR;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class AddOrUpdateApprenticeTaskCommand : IRequest<Unit>
    {
        public Guid ApprenticeshipId { get; set; }
        public ApprenticeTaskData Data { get; set; }
    }
}
