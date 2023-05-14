using MediatR;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerProfiles.Application.AccountUsers.Commands
{
    public class UpsertAccountCommand : IRequest<EmployerProfile>
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GovIdentifier { get; set; }
    }
}
