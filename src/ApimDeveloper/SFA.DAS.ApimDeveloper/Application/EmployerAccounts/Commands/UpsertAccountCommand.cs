using MediatR;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Commands
{
    public class UpsertAccountCommand : IRequest<EmployerProfile>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GovIdentifier { get; set; }
    }
}
