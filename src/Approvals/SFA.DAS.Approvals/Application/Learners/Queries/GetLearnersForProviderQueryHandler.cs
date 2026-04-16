using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Reservations;
using SFA.DAS.SharedOuterApi.Interfaces;
using GetAllStandardsRequest = SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses.GetAllStandardsRequest;

namespace SFA.DAS.Approvals.Application.Learners.Queries;

public class GetLearnersForProviderQueryHandler(
    IInternalApiClient<LearnerDataInnerApiConfiguration> learnerDataClient,
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsClient,
    IMapLearnerRecords mapper,
    IReservationApiClient<ReservationApiConfiguration> _reservationsApiClient,
ILogger<GetLearnersForProviderQueryHandler> logger)
    : IRequestHandler<GetLearnersForProviderQuery, GetLearnersForProviderQueryResult>
{
    public async Task<GetLearnersForProviderQueryResult> Handle(GetLearnersForProviderQuery request,
        CancellationToken cancellationToken)
    {
        long accountLegalEntityId = 0;
        string employerName = null;
        int futureMonths = 0;

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

            if (legalEntityResponse.Body.LevyStatus == ApprenticeshipEmployerType.NonLevy)
            {
                var get = new GetAvailableDatesRequest(accountLegalEntityId);
                var response = await _reservationsApiClient.Get<GetAvailableDatesResponse>(get);
                if (response.AvailableDates is not null)
                {
                    futureMonths = response.AvailableDates.Count(x => x.StartDate > DateTime.Now) + 1;

                    var maxStartDate = response.AvailableDates.Max(x => x.StartDate).AddMonths(1);
                    request.MaxStartDate = maxStartDate;
                }
            }
        }

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
            var draftApprenticeshipsResponse = await commitmentsClient.GetWithResponseCode<GetDraftApprenticeshipsResponse>(apiRequest);
            if (!string.IsNullOrEmpty(draftApprenticeshipsResponse.ErrorContent))
            {
                throw new ApplicationException($"Getting Draft Apprenticeships Failed. Status Code {draftApprenticeshipsResponse.StatusCode} Error : {draftApprenticeshipsResponse.ErrorContent}");
            }
            var selectedUlns = draftApprenticeshipsResponse.Body.DraftApprenticeships.Select(x => x.Uln).ToList();
            request.ExcludeUlns = selectedUlns;
        }

        var learnerDataTask = GetLearnerData(request);
        var standardsTask = GetStandardsData();
        var coursesTask = GetCourses(request.ProviderId);

        await Task.WhenAll(learnerDataTask, standardsTask, coursesTask);
        var learnerData = learnerDataTask.Result;
        var standards = standardsTask.Result;
        var courses = coursesTask.Result;

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
            Learners = await mapper.Map(learnerData.Data, standards.TrainingProgrammes.ToList()),
            FutureMonths = futureMonths,
            TrainingCourses = courses.ToList()
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
                request.MaxStartDate,
                string.Join(",", request.ExcludeUlns),
                request.CourseCode
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

    private async Task<IEnumerable<TrainingProgramme>> GetCourses(long ukprn)
    {
        var response = await commitmentsClient.GetWithResponseCode<GetCourseCodesResponse>(new GetAllTrainingProgrammesRequest());

        var courseCodes = await learnerDataClient.GetWithResponseCode<GetCourseCodesByUkprnResponse>(new GetCourseCodesByUkprnRequest(ukprn));

        var codes = courseCodes.Body.CourseCodes.Select(t => t.ToString());

        var trainingProgrammesForUkprn = response.Body.TrainingProgrammes.Where(t => codes.Contains(t.CourseCode));

        return trainingProgrammesForUkprn;
    }
}