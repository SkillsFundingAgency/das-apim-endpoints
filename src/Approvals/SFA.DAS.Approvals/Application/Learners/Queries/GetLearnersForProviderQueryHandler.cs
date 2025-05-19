﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Learners.Queries;

public class GetLearnersForProviderQueryHandler(
    IInternalApiClient<LearnerDataInnerApiConfiguration> learnerDataClient,
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsClient,
    IMapLearnerRecords mapper,
    ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
    ILogger<GetLearnersForProviderQueryHandler> logger)
    : IRequestHandler<GetLearnersForProviderQuery, GetLearnersForProviderQueryResult>
{
    public async Task<GetLearnersForProviderQueryResult> Handle(GetLearnersForProviderQuery request,
        CancellationToken cancellationToken)
    {
        long accountLegalEntityId = 0;
        string employerName = null;

        var learnerDataTask = GetLearnerData(request);
        var standardsTask = GetStandardsData();

        await Task.WhenAll(learnerDataTask, standardsTask);
        var learnerData = await learnerDataTask;
        var standards = await standardsTask;

        if (request.CohortId.HasValue)
        {
            logger.LogInformation("Getting Cohort details");
            var cohortResponse = await commitmentsClient.GetWithResponseCode<GetCohortResponse>(
                new GetCohortRequest(request.CohortId.Value));

            if (!string.IsNullOrEmpty(cohortResponse.ErrorContent))
            {
                throw new ApplicationException($"Getting Cohort Failed. Status Code {cohortResponse.StatusCode} Error : {cohortResponse.ErrorContent}");
            }
            employerName = cohortResponse.Body.LegalEntityName;
            accountLegalEntityId = cohortResponse.Body.AccountLegalEntityId;
        }

        if (request.AccountLegalEntityId.HasValue)
        {
            logger.LogInformation("Getting Account Legal Entity");
            accountLegalEntityId = request.AccountLegalEntityId.Value;
            var legalEntityResponse = await commitmentsClient.GetWithResponseCode<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(accountLegalEntityId));
            if (!string.IsNullOrEmpty(legalEntityResponse.ErrorContent))
            {
                throw new ApplicationException($"Getting Legal Entity Data Failed. Status Code {legalEntityResponse.StatusCode} Error : {legalEntityResponse.ErrorContent}");
            }
            employerName = legalEntityResponse.Body.LegalEntityName;
        }

        logger.LogInformation("Building Learner Data result");

        return new GetLearnersForProviderQueryResult
        {
            LastSubmissionDate = learnerData.LastSubmissionDate,
            Total = learnerData.TotalItems,
            AccountLegalEntityId = accountLegalEntityId,
            EmployerName = employerName,
            Page = learnerData.Page,
            PageSize = learnerData.PageSize,
            TotalPages = learnerData.TotalPages,
            Learners = await mapper.Map(learnerData.Data, standards.Standards.ToList())
        };
    }

    private async Task<GetLearnersForProviderResponse> GetLearnerData(GetLearnersForProviderQuery request)
    {
        logger.LogInformation("Getting Learner Data for Provider {ProviderId}", request.ProviderId);

        var response = await learnerDataClient.GetWithResponseCode<GetLearnersForProviderResponse>(
            new GetLearnersForProviderRequest(
                request.ProviderId,
                2425,
                request.SearchTerm,
                request.SortField,
                request.SortDescending,
                request.Page,
                request.PageSize
            ));

        if (!string.IsNullOrEmpty(response.ErrorContent))
        {
            throw new ApplicationException($"Getting Learner Data Failed, Status Code {response.StatusCode} Error : {response.ErrorContent}");
        }

        return response.Body;
    }

    private async Task<GetStandardsListResponse> GetStandardsData()
    {
        logger.LogInformation("Getting All Courses");
        var response = await coursesApiClient.GetWithResponseCode<GetStandardsListResponse>(new GetStandardsExportRequest());

        if (!string.IsNullOrEmpty(response.ErrorContent))
        {
            throw new ApplicationException($"Getting all courses Failed. Status Code {response.StatusCode} Error : {response.ErrorContent}");
        }

        return response.Body;
    }

}