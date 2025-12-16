using MediatR;
using SFA.DAS.ApprenticeApp.Models;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class DeleteApprenticeAccountCommand : IRequest<DeleteApprenticeAccountResponse>
    {
        public Guid ApprenticeId { get; set; }
    }
}
