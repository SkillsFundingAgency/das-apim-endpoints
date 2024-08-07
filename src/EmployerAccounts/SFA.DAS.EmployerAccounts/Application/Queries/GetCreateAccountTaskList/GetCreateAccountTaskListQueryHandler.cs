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
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PayeSchemes;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.User;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAgreements;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerRegistration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PayeSchemes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.User;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetCreateAccountTaskList;

public class GetCreateAccountTaskListQueryHandler(
    IAccountsApiClient<AccountsConfiguration> accountsApiClient,
    IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipsApiClient,
    ILogger<GetCreateAccountTaskListQueryHandler> logger) : IRequestHandler<GetCreateAccountTaskListQuery, GetCreateAccountTaskListQueryResponse>
{
    public async Task<GetCreateAccountTaskListQueryResponse> Handle(GetCreateAccountTaskListQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("{HandlerName}: Processing started for request: {Request}.", nameof(GetCreateAccountTaskListQueryHandler), JsonSerializer.Serialize(request));

        GetCreateAccountTaskListQueryResponse response;

        var userResponse = await accountsApiClient.Get<GetUserByRefResponse>(new GetUserByRefRequest(request.UserRef));

        if (string.IsNullOrEmpty(request.HashedAccountId))
        {
            response = await CreateResponseFromNewestAccountFor(request.UserRef);

            if (response == null)
            {
                logger.LogInformation("{HandlerName}: Result from {MethodName} is null. Returning null.", nameof(GetCreateAccountTaskListQueryHandler), nameof(CreateResponseFromNewestAccountFor));
                return null;
            }

            response.UserFirstName = userResponse.FirstName;
            response.UserLastName = userResponse.LastName;

            return response;
        }

        logger.LogInformation("{HandlerName}: Retrieving data.", nameof(GetCreateAccountTaskListQueryHandler));

        var (taskListResponse, accountResponse, accountAgreementsResponse) = await GetData(request);

        var agreement = accountAgreementsResponse.FirstOrDefault();

        if (accountResponse == null || agreement == null)
        {
            logger.LogWarning("{HandlerName}: Returning null. Account Response null: {AccountIsNull}. Agreement null: {CountIsZero}",
                nameof(GetCreateAccountTaskListQueryHandler),
                accountResponse == null,
                agreement == null);

            return null;
        }
        
        var payeSchemes = (await accountsApiClient.GetAll<GetAccountPayeSchemesResponse>(new GetAccountPayeSchemesRequest(request.AccountId))).ToList();
        
        response = BuildResponse(request,
            payeSchemes,
            accountResponse,
            agreement,
            userResponse,
            taskListResponse.hasProviders,
            taskListResponse.hasPermissions);

        await AcknowledgeTrainingProviderTaskIfOutstanding(request, response);

        return response;
    }

    private async Task AcknowledgeTrainingProviderTaskIfOutstanding(GetCreateAccountTaskListQuery request, GetCreateAccountTaskListQueryResponse taskListResponse)
    {
        if (!taskListResponse.AddTrainingProviderAcknowledged.GetValueOrDefault() && taskListResponse.HasProviders && taskListResponse.HasProviderPermissions)
        {
            logger.LogInformation("{HandlerName}: Executing AcknowledgeTrainingProviderTaskRequest.", nameof(GetCreateAccountTaskListQueryHandler));

            await accountsApiClient.Patch(new AcknowledgeTrainingProviderTaskRequest(new AcknowledgeTrainingProviderTaskData(request.AccountId)));
            
            taskListResponse.AddTrainingProviderAcknowledged = true;
        }
    }

    private async Task<((bool hasProviders, bool hasPermissions),
            GetAccountByIdResponse accountResponse,
            IEnumerable<GetEmployerAgreementsResponse> accountAgreementsResponse)>
        GetData(GetCreateAccountTaskListQuery request)
    {
        var taskListTask = GetTaskList(request.AccountId, request.HashedAccountId);
        var accountResponseTask = accountsApiClient.Get<GetAccountByIdResponse>(new GetAccountByIdRequest(request.AccountId));
        var accountAgreementsResponseTask = accountsApiClient.GetAll<GetEmployerAgreementsResponse>(new GetEmployerAgreementsRequest(request.AccountId));

        await Task.WhenAll(taskListTask, accountResponseTask, accountAgreementsResponseTask);
        
        return (taskListTask.Result,
            accountResponseTask.Result,
            accountAgreementsResponseTask.Result);
    }

    private async Task<GetCreateAccountTaskListQueryResponse> CreateResponseFromNewestAccountFor(string userRef)
    {
        logger.LogInformation("{HandlerName}: Processing {MethodName}.", nameof(GetCreateAccountTaskListQueryHandler), nameof(CreateResponseFromNewestAccountFor));

        var userAccounts = (await accountsApiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(userRef))).ToList();

        var firstAccount = userAccounts.Count == 0
            ? null
            : userAccounts.MinBy(x => x.DateRegistered);

        if (firstAccount == null)
        {
            logger.LogInformation("{HandlerName}: No account found. Returning null.", nameof(GetCreateAccountTaskListQueryHandler));
            return null;
        }
        
        var payeSchemes = await accountsApiClient.GetAll<GetAccountPayeSchemesResponse>(new GetAccountPayeSchemesRequest(firstAccount.AccountId));

        return new GetCreateAccountTaskListQueryResponse
        {
            HashedAccountId = firstAccount.EncodedAccountId,
            HasPayeScheme = payeSchemes != null && payeSchemes.Any(),
            NameConfirmed = firstAccount.NameConfirmed
        };
    }

    private async Task<(bool hasProviders, bool hasPermissions)> GetTaskList(long accountId, string hashedAccountId)
    {
        var accountProvidersResponse =
            await providerRelationshipsApiClient.Get<GetAccountProvidersResponse>(
                new GetAccountProvidersRequest(accountId));

        if (accountProvidersResponse?.AccountProviders?.Count == 0)
        {
            return (
                hasProviders: false,
                hasPermissions: false
            );
        }

        var providerRelationshipResponse =
            await providerRelationshipsApiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                new GetEmployerAccountProviderPermissionsRequest(hashedAccountId));

        var employerAlePermissions = providerRelationshipResponse?.AccountProviderLegalEntities?.Select(legalEntityItem => new AccountLegalEntityItem
        {
            Name = legalEntityItem.AccountLegalEntityName,
            AccountLegalEntityPublicHashedId = legalEntityItem.AccountLegalEntityPublicHashedId,
            AccountHashedId = legalEntityItem.AccountHashedId
        });

        return (
            hasProviders: true,
            hasPermissions: employerAlePermissions != null && employerAlePermissions.Any()
        );
    }

    private static GetCreateAccountTaskListQueryResponse BuildResponse(GetCreateAccountTaskListQuery request,
        IEnumerable<GetAccountPayeSchemesResponse> payeSchemes,
        GetAccountByIdResponse accountResponse,
        GetEmployerAgreementsResponse agreement,
        GetUserByRefResponse userResponse,
        bool hasProviders,
        bool hasPermissions)
    {
        return new GetCreateAccountTaskListQueryResponse
        {
            HashedAccountId = request.HashedAccountId,
            HasPayeScheme = payeSchemes.Any(),
            NameConfirmed = accountResponse.NameConfirmed,
            PendingAgreementId = agreement.Id,
            AddTrainingProviderAcknowledged = accountResponse.AddTrainingProviderAcknowledged,
            UserFirstName = userResponse.FirstName,
            UserLastName = userResponse.LastName,
            AgreementAcknowledged = agreement.Acknowledged ?? true,
            HasSignedAgreement = agreement.SignedDate.HasValue,
            HasProviders = hasProviders,
            HasProviderPermissions = hasPermissions
        };
    }
}