using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Learners.Queries;

public class GetLearnersForProviderQueryHandler(
    IInternalApiClient<LearnerDataInnerApiConfiguration> learnerDataClient,
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsClient,
    IMapLearnerRecords mapper, 
    ILogger<GetLearnersForProviderQueryHandler> logger)
    : IRequestHandler<GetLearnersForProviderQuery, GetLearnersForProviderQueryResult>
{
    public async Task<GetLearnersForProviderQueryResult> Handle(GetLearnersForProviderQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Learner Data for Provider {0}", request.ProviderId);
        var learnerDataResponseTask = learnerDataClient.GetWithResponseCode<GetLearnersForProviderResponse>(
            new GetLearnersForProviderRequest(
                request.ProviderId,
                2425,
                request.SearchTerm,
                request.SortField,
                request.SortDescending,
                request.Page,
                request.PageSize
            ));

        logger.LogInformation("Getting Account Legal Entity");
        var legalEntityResponseTask = commitmentsClient.GetWithResponseCode<GetAccountLegalEntityResponse>(
                new GetAccountLegalEntityRequest(request.AccountLegalEntityId));

        await Task.WhenAll(learnerDataResponseTask, legalEntityResponseTask);

        var learnerDataResponse = await learnerDataResponseTask;
        var legalEntityResponse = await legalEntityResponseTask;

        if (!string.IsNullOrEmpty(learnerDataResponse.ErrorContent))
        {
            throw new ApplicationException($"Getting Learner Data Failed, Status Code {learnerDataResponse.StatusCode} Error : {learnerDataResponse.ErrorContent}");
        }
        if (!string.IsNullOrEmpty(legalEntityResponse.ErrorContent))
        {
            throw new ApplicationException($"Getting Legal Entity Data Failed. Status Code {legalEntityResponse.StatusCode} Error : {legalEntityResponse.ErrorContent}");
        }

        logger.LogInformation("Building Learner Data result");

        var response = learnerDataResponse.Body;
        return new GetLearnersForProviderQueryResult
        {
            LastSubmissionDate = response.LastSubmissionDate,
            Total = response.TotalItems,
            AccountLegalEntityId = request.AccountLegalEntityId,
            EmployerName = legalEntityResponse.Body.LegalEntityName,
            Page = response.Page,
            PageSize = response.PageSize,
            TotalPages = response.TotalPages,
            Learners = await mapper.Map(response.Data)
        };
    }
}