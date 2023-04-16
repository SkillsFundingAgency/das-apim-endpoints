﻿using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Commands
{
    public class UpsertAccountCommandHandler : IRequestHandler<UpsertAccountCommand, EmployerProfile>
    {
        private readonly IEmployerAccountsService _employerAccountService;

        public UpsertAccountCommandHandler(IEmployerAccountsService employerAccountService)
        {
            _employerAccountService = employerAccountService;
        }
        public async Task<EmployerProfile> Handle(UpsertAccountCommand request, CancellationToken cancellationToken)
        {
            return await _employerAccountService.PutEmployerAccount(new EmployerProfile
            {
                Email = request.Email,
                UserId = request.GovIdentifier,
                FirstName = request.FirstName,
                LastName = request.LastName,
            });
        }
    }
}
