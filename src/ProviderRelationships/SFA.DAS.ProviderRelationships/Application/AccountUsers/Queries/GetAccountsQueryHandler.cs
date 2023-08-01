﻿using MediatR;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.ProviderRelationships.Application.AccountUsers.Queries;

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, GetAccountsQueryResult>
{
    private readonly IEmployerAccountsService _employerAccountsService;
    
    public GetAccountsQueryHandler(IEmployerAccountsService employerAccountsService)
    {
        _employerAccountsService = employerAccountsService;
    }
    
    public async Task<GetAccountsQueryResult> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        var employerAccounts = (await _employerAccountsService.GetEmployerAccounts(new EmployerProfile
        {
            Email = request.Email,
            UserId = request.UserId
        })).ToList();
            
        return new GetAccountsQueryResult
        {
            EmployerUserId = employerAccounts.FirstOrDefault()?.UserId,
            FirstName = employerAccounts.FirstOrDefault()?.FirstName,
            LastName = employerAccounts.FirstOrDefault()?.LastName,
            IsSuspended = employerAccounts.FirstOrDefault()?.IsSuspended ?? false,
            UserAccountResponse = employerAccounts.Where(c=>c.EncodedAccountId != null).Select(c=> new AccountUser
            {
                DasAccountName = c.DasAccountName,
                EncodedAccountId = c.EncodedAccountId,
                Role = c.Role
            })
        };
    }
}