using MediatR;
using SFA.DAS.ApprenticeApp.Models;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Commands.Cmad
{
    public class CreateMyApprenticeshipCommand : IRequest<Unit>
    {
        public Guid ApprenticeId { get; set; }
        public CreateMyApprenticeshipData Data { get; set; }
    }
}
