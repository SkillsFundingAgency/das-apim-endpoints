using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GetAllStandardsRequest = SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses.GetAllStandardsRequest;

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
        List<string> selectedUlns = new List<string>();

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

            //find draft apprenticeships in the cohort to exclude from the learner search results
            var apiRequest = new GetDraftApprenticeshipsRequest(request.CohortId.Value);
            var draftApprenticeships = await commitmentsClient.GetWithResponseCode<GetDraftApprenticeshipsResponse>(apiRequest);
            selectedUlns = draftApprenticeships.Body.DraftApprenticeships.Select(x => x.Uln).ToList();
            request.ExcludeUlns = selectedUlns;
        }        

        var learnerDataTask = GetLearnerData(request);
        var standardsTask = GetStandardsData();

        await Task.WhenAll(learnerDataTask, standardsTask);
        var learnerData = await learnerDataTask;
        var standards = await standardsTask;        

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
            Learners = await mapper.Map(learnerData.Data, standards.TrainingProgrammes.ToList())
        };
    }

    private async Task<GetLearnersForProviderResponse> GetLearnerData(GetLearnersForProviderQuery request)
    {
        logger.LogInformation("Getting Learner Data for Provider {ProviderId}", request.ProviderId);

        var response = await learnerDataClient.GetWithResponseCode<GetLearnersForProviderResponse>(
            new GetLearnersForProviderRequest(
                request.ProviderId,
                request.SearchTerm,
                request.SortField,
                request.SortDescending,
                request.Page,
                request.PageSize, 
                request.StartMonth,
                request.StartYear,
                string.Join(",", request.ExcludeUlns)
            ));

        if (!string.IsNullOrEmpty(response.ErrorContent))
        {
            throw new ApplicationException($"Getting Learner Data Failed, Status Code {response.StatusCode} Error : {response.ErrorContent}");
        }

        return response.Body;
    }

    private async Task<GetAllStandardsResponse> GetStandardsData()
    {
        logger.LogInformation("Getting All Courses");

        var response = await commitmentsClient.GetWithResponseCode<GetAllStandardsResponse>(new GetAllStandardsRequest());

        if (!string.IsNullOrEmpty(response.ErrorContent))
        {
            throw new ApplicationException($"Getting all courses Failed. Status Code {response.StatusCode} Error : {response.ErrorContent}");
        }

        return response.Body;
    }
}