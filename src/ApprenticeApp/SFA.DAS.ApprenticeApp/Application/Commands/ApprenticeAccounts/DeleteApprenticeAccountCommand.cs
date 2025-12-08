using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class DeleteApprenticeAccountCommand : IRequest<Unit>
    {
        public Guid ApprenticeId { get; set; }
    }
}
