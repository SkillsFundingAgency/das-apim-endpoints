using MediatR;
using System;

namespace SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Commands.UpsertEmployer
{
    public class UpsertAccountCommand : IRequest<UpsertAccountCommandResult>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GovIdentifier { get; set; }
    }
}
