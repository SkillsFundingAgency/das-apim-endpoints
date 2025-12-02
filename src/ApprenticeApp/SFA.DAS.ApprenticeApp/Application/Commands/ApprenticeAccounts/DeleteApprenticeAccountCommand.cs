using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class DeleteApprenticeAccountCommand : IRequest<Unit>
    {
        public Guid ApprenticeId { get; set; }
    }
}
