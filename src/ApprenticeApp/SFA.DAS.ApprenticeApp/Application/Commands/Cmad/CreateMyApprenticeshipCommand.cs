using MediatR;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Models;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Commands.Cmad
{
    public class CreateMyApprenticeshipCommand : IRequest<ApiResponse<object>>
    {
        public Guid ApprenticeId { get; set; }
        public CreateMyApprenticeshipData Data { get; set; }
    }
}
