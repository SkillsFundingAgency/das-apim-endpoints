using MediatR;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Commands.UpsertEmployer
{
    public class UpsertAccountCommandHandler : IRequestHandler<UpsertAccountCommand, UpsertAccountCommandResult>
    {
        private readonly IEmployerAccountsService _employerAccountService;

        public UpsertAccountCommandHandler(IEmployerAccountsService employerAccountService)
        {
            _employerAccountService = employerAccountService;
        }
        public async Task<UpsertAccountCommandResult> Handle(UpsertAccountCommand request, CancellationToken cancellationToken)
        {
            var actual = await _employerAccountService.PutEmployerAccount(new EmployerProfile
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserId = request.Id.ToString()
            });

            return new UpsertAccountCommandResult
            {
                FirstName = actual.FirstName,
                LastName = actual.LastName,
                Email = actual.Email,
                UserId = actual.UserId
            };
        }
    }
}
