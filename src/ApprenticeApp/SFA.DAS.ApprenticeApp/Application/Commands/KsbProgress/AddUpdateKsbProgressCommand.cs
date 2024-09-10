using System;
using MediatR;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Application.Commands
{
    public class AddUpdateKsbProgressCommand : IRequest<Unit>
    {
        public long ApprenticeshipId { get; set; }
        public ApprenticeKsbProgressData Data { get; set; }
    }
}
