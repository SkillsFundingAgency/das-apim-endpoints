﻿using MediatR;
using SFA.DAS.ToolsSupport.Configuration;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetPayeSchemeLevyDeclarations;

public class GetPayeSchemeLevyDeclarationsQueryHandler(
    IAccountsService accountsService,
    IPayRefHashingService hashingService, IHmrcApiClient<HmrcApiConfiguration> hmrcApiClient) : IRequestHandler<GetPayeSchemeLevyDeclarationsQuery, GetPayeSchemeLevyDeclarationsResult>
{
    public async Task<GetPayeSchemeLevyDeclarationsResult> Handle(GetPayeSchemeLevyDeclarationsQuery query, CancellationToken cancellationToken)
    {
        var result = new GetPayeSchemeLevyDeclarationsResult();

        var account = await accountsService.GetAccount(query.AccountId);
        if (account == null)
        {
            result.StatusCode = PayeLevySubmissionsResponseCodes.AccountNotFound;
            return result;
        }

        var actualPayeId = hashingService.DecodeValueToString(query.HashedPayeRef);

        result.PayeScheme = await GetSelectedPayeSchemeFromAccount(account, actualPayeId);
        result.LevySubmissions = await GetLevyDeclarationsForPayeScheme(actualPayeId);
        result.StatusCode = PayeLevySubmissionsResponseCodes.Success;

        return result;
    }

    private static List<Declaration> GetFilteredDeclarations(List<Declaration> resultDeclarations)
    {
        return resultDeclarations?.Where(x => x.SubmissionTime >= new DateTime(2017, 4, 1)).ToList();
    }

    private async Task<PayeScheme> GetSelectedPayeSchemeFromAccount(Account account, string payeId)
    {
        var payeScheme = account.PayeSchemes.First(x => x.Id == payeId);

        return await accountsService.GetEmployerAccountPayeScheme(payeScheme.Href);
    }

    private async Task<PayeSchemeLevyDeclarations> GetLevyDeclarationsForPayeScheme(string payeId)
    {
        var payeResponse = await hmrcApiClient.Get<PayeSchemeLevyDeclarations>(new GetPayeSchemeLevyDeclarationsRequest(payeId));
        if (payeResponse != null)
        {
            var filteredDeclarations = GetFilteredDeclarations(payeResponse.Declarations);
            payeResponse.Declarations = [.. filteredDeclarations.OrderByDescending(x => x.SubmissionTime).ThenByDescending(x => x.Id)];
        }

        return payeResponse ?? new();
    }
}