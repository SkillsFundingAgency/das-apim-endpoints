using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAgreements;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.GetEmployerAccountTaskList;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PayeSchemes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAgreements;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerRegistration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.GetEmployerAccountTaskList;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PayeSchemes;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetCreateAccountTaskList;

public class GetCreateAccountTaskListQueryHandler(
    IAccountsApiClient<AccountsConfiguration> accountsApiClient,
    ILogger<GetCreateAccountTaskListQueryHandler> logger) : IRequestHandler<GetCreateAccountTaskListQuery, GetCreateAccountTaskListQueryResponse>
{
    public async Task<GetCreateAccountTaskListQueryResponse> Handle(GetCreateAccountTaskListQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("{HandlerName}: Processing started for request: {Request}.", nameof(GetCreateAccountTaskListQueryHandler), JsonSerializer.Serialize(request));
        
        if (string.IsNullOrEmpty(request.HashedAccountId))
        {
            logger.LogInformation("{HandlerName}: HashedAccountId IsNullOrEmpty. Creating response from newest account.", nameof(GetCreateAccountTaskListQueryHandler));
            return await CreateResponseFromNewestAccountFor(request.UserRef);
        }

        logger.LogInformation("{HandlerName}: Retrieving data.", nameof(GetCreateAccountTaskListQueryHandler));
        
        var (taskListResponse, accountResponse, accountAgreementsResponse) = await GetData(request);

        if (accountResponse == null || accountAgreementsResponse.Count == 0)
        {
            logger.LogWarning("{HandlerName}: Returning null. Account Response null: {AccountIsNull}. Account Agreements Count == 0: {CountIsZero}",
                nameof(GetCreateAccountTaskListQueryHandler),
                accountResponse == null,
                accountAgreementsResponse.Count == 0);

            return null;
        }

        logger.LogInformation("{HandlerName}: Retrieving PAYE Schemes.", nameof(GetCreateAccountTaskListQueryHandler));
        var payeSchemes = await accountsApiClient.GetAll<GetAccountPayeSchemesResponse>(new GetAccountPayeSchemesRequest(request.HashedAccountId));

        var agreement = accountAgreementsResponse.FirstOrDefault();

        logger.LogInformation("{HandlerName}: Building Response.", nameof(GetCreateAccountTaskListQueryHandler));
        
        var response = BuildResponse(request,
            payeSchemes,
            accountResponse,
            agreement,
            taskListResponse);

        await AcknowledgeTrainingProviderTaskIfRequired(request, agreement, response);

        return response;
    }

    private async Task AcknowledgeTrainingProviderTaskIfRequired(GetCreateAccountTaskListQuery request, GetEmployerAgreementsResponse agreement, GetCreateAccountTaskListQueryResponse response)
    {
        if (agreement != null && !response.AddTrainingProviderAcknowledged && response.HasProviders && response.HasProviderPermissions)
        {
            logger.LogInformation("{HandlerName}: Executing AcknowledgeTrainingProviderTaskRequest.", nameof(GetCreateAccountTaskListQueryHandler));
            
            await accountsApiClient.Patch(new AcknowledgeTrainingProviderTaskRequest
            {
                Data = new AcknowledgeTrainingProviderTaskData(request.AccountId)
            });
        }
    }

    private async Task<(GetEmployerAccountTaskListResponse taskListResponse,
            GetAccountByHashedIdResponse accountResponse,
            List<GetEmployerAgreementsResponse> accountAgreementsResponse)>
        GetData(GetCreateAccountTaskListQuery request)
    {
        var taskListTask = accountsApiClient.Get<GetEmployerAccountTaskListResponse>(new GetEmployerAccountTaskListRequest(request.AccountId, request.HashedAccountId));
        var accountResponseTask = accountsApiClient.Get<GetAccountByHashedIdResponse>(new GetAccountByHashedIdRequest(request.HashedAccountId));
        var accountAgreementsResponseTask = accountsApiClient.GetAll<GetEmployerAgreementsResponse>(new GetEmployerAgreementsRequest(request.AccountId));

        logger.LogInformation("{HandlerName}: Awaiting GetData tasks.", nameof(GetCreateAccountTaskListQueryHandler));
        
        await Task.WhenAll(taskListTask, accountResponseTask, accountAgreementsResponseTask);

        logger.LogInformation("{HandlerName}: GetData tasks completed.", nameof(GetCreateAccountTaskListQueryHandler));
        
        return (taskListTask.Result,
            accountResponseTask.Result,
            accountAgreementsResponseTask.Result.ToList());
    }

    private async Task<GetCreateAccountTaskListQueryResponse> CreateResponseFromNewestAccountFor(string userRef)
    {
        logger.LogInformation("{HandlerName}: Processing {Request}.", nameof(GetCreateAccountTaskListQueryHandler), nameof(GetUserAccountsRequest));
        
        var userAccounts = (await accountsApiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(userRef))).ToList();

        var firstAccount = userAccounts.Count == 0
            ? null
            : userAccounts.MinBy(x => x.DateRegistered);

        var payeSchemes = new List<GetAccountPayeSchemesResponse>();

        if (firstAccount != null)
        {
            logger.LogInformation("{HandlerName}: Processing {Request}.", nameof(GetCreateAccountTaskListQueryHandler), nameof(GetAccountPayeSchemesRequest));
            payeSchemes = (await accountsApiClient.GetAll<GetAccountPayeSchemesResponse>(new GetAccountPayeSchemesRequest(firstAccount.EncodedAccountId))).ToList();
        }

        return new GetCreateAccountTaskListQueryResponse
        {
            HashedAccountId = firstAccount?.EncodedAccountId,
            HasPayeScheme = payeSchemes.Count > 0,
            NameConfirmed = firstAccount?.NameConfirmed ?? false
        };
    }

    private static GetCreateAccountTaskListQueryResponse BuildResponse(
        GetCreateAccountTaskListQuery request,
        IEnumerable<GetAccountPayeSchemesResponse> payeSchemes,
        GetAccountByHashedIdResponse accountResponse,
        GetEmployerAgreementsResponse agreement,
        GetEmployerAccountTaskListResponse employerAccountTaskListResponse)
    {
        var hasAgreement = agreement != null;

        var response = new GetCreateAccountTaskListQueryResponse
        {
            HashedAccountId = request.HashedAccountId,
            HasPayeScheme = payeSchemes.Any(),
            NameConfirmed = accountResponse.NameConfirmed,
            PendingAgreementId = hasAgreement ? agreement.Id : null,
            AddTrainingProviderAcknowledged = accountResponse.AddTrainingProviderAcknowledged
        };

        if (!hasAgreement)
        {
            return response;
        }

        response.AgreementAcknowledged = agreement.Acknowledged ?? true;
        response.HasSignedAgreement = agreement.SignedDate.HasValue;
        response.HasProviders = employerAccountTaskListResponse?.HasProviders ?? false;
        response.HasProviderPermissions = employerAccountTaskListResponse?.HasPermissions ?? false;

        return response;
    }
}