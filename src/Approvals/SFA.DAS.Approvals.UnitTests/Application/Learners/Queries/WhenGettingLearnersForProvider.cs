using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.Learners.Queries;
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
using SFA.DAS.SharedOuterApi.Models;
using GetAllStandardsRequest = SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses.GetAllStandardsRequest;

namespace SFA.DAS.Approvals.UnitTests.Application.Learners.Queries;

public class WhenGettingLearnersForProvider
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_With_LegalEntityId_And_Clients_Return_Responses_The_result_Is_Returned(
        GetLearnersForProviderQuery query,
        GetLearnersForProviderRequest learnersRequest,
        GetLearnersForProviderResponse learnersResponse,
        GetAccountLegalEntityRequest aleRequest,
        GetAccountLegalEntityResponse aleResponse,
        GetAllStandardsRequest coursesRequest,
        GetAllStandardsResponse coursesResponse,
        GetCourseCodesResponse courseCodesResponse,
        GetCourseCodesByUkprnResponse courseCodesByUkprnResponse,
        List<LearnerSummary> learners,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IMapLearnerRecords> mapper,
        [Greedy] GetLearnersForProviderQueryHandler handler
    )
    {
        query.CohortId = null;
        GetLearnersForProviderRequest input;
        learnerDataClient.Setup(x =>
                x.GetWithResponseCode<GetLearnersForProviderResponse>(It.IsAny<GetLearnersForProviderRequest>()))
            .ReturnsAsync(new ApiResponse<GetLearnersForProviderResponse>(learnersResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x =>
                x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
            .ReturnsAsync(new ApiResponse<GetAccountLegalEntityResponse>(aleResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x => x.GetWithResponseCode<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(coursesResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x =>
          x.GetWithResponseCode<GetCourseCodesResponse>(It.IsAny<GetAllTrainingProgrammesRequest>()))
       .ReturnsAsync(new ApiResponse<GetCourseCodesResponse>(courseCodesResponse, HttpStatusCode.OK, null));

        learnerDataClient.Setup(x => x.GetWithResponseCode<GetCourseCodesByUkprnResponse>(It.Is<GetCourseCodesByUkprnRequest>(t => t.Ukprn == query.ProviderId)))
            .ReturnsAsync(new ApiResponse<GetCourseCodesByUkprnResponse>(courseCodesByUkprnResponse, HttpStatusCode.OK, null));

        mapper.Setup(x => x.Map(learnersResponse.Data, It.IsAny<List<GetAllStandardsResponse.TrainingProgramme>>())).ReturnsAsync(learners);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Should().NotBeNull();
        actual.AccountLegalEntityId.Should().Be(query.AccountLegalEntityId);
        actual.EmployerName.Should().Be(aleResponse.LegalEntityName);
        actual.LastSubmissionDate.Should().Be(learnersResponse.LastSubmissionDate);
        actual.Page.Should().Be(learnersResponse.Page);
        actual.PageSize.Should().Be(learnersResponse.PageSize);
        actual.Total.Should().Be(learnersResponse.TotalItems);
        actual.TotalPages.Should().Be(learnersResponse.TotalPages);
        actual.Learners.Should().BeEquivalentTo(learners);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_With_LegalEntityId_For_NonLevyEmp_And_Clients_Return_Responses_The_result_Is_Returned_And_FutureMonths_Is_Populated(
        GetLearnersForProviderQuery query,
        GetLearnersForProviderRequest learnersRequest,
        GetLearnersForProviderResponse learnersResponse,
        GetAccountLegalEntityRequest aleRequest,
        GetAccountLegalEntityResponse aleResponse,
        GetAllStandardsRequest coursesRequest,
        GetAllStandardsResponse coursesResponse,
        List<LearnerSummary> learners,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationsClient,
        [Frozen] Mock<IMapLearnerRecords> mapper,
        [Greedy] GetLearnersForProviderQueryHandler handler
    )
    {
        query.CohortId = null;
        GetLearnersForProviderRequest input;
        aleResponse.LevyStatus = ApprenticeshipEmployerType.NonLevy;

        GetAvailableDatesResponse availableDatesResponse = new GetAvailableDatesResponse
        {
            AvailableDates = new List<AvailableDateStartWindow>
            {
                new AvailableDateStartWindow { StartDate = DateTime.Now },
                new AvailableDateStartWindow { StartDate = DateTime.Now.AddMonths(1) },
                new AvailableDateStartWindow { StartDate = DateTime.Now.AddMonths(2) }
            }
        };

        GetCourseCodesResponse courseCodesResponse = new GetCourseCodesResponse()
        {
            TrainingProgrammes = [
                 new TrainingProgramme() { CourseCode = "4" , Name = "course4" },
                 new TrainingProgramme() { CourseCode = "5" , Name = "course5" },
                 new TrainingProgramme() { CourseCode = "6" , Name = "course6" }
             ]
        };

        GetCourseCodesByUkprnResponse courseCodesByUkprnResponse = new GetCourseCodesByUkprnResponse()
        {
            CourseCodes = [4, 5, 6]
        };

        learnerDataClient.Setup(x =>
                x.GetWithResponseCode<GetLearnersForProviderResponse>(It.IsAny<GetLearnersForProviderRequest>()))
            .ReturnsAsync(new ApiResponse<GetLearnersForProviderResponse>(learnersResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x =>
                x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
            .ReturnsAsync(new ApiResponse<GetAccountLegalEntityResponse>(aleResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x => x.GetWithResponseCode<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(coursesResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x =>
             x.GetWithResponseCode<GetCourseCodesResponse>(It.IsAny<GetAllTrainingProgrammesRequest>()))
            .ReturnsAsync(new ApiResponse<GetCourseCodesResponse>(courseCodesResponse, HttpStatusCode.OK, null));

        learnerDataClient.Setup(x => x.GetWithResponseCode<GetCourseCodesByUkprnResponse>(It.Is<GetCourseCodesByUkprnRequest>(t => t.Ukprn == query.ProviderId)))
            .ReturnsAsync(new ApiResponse<GetCourseCodesByUkprnResponse>(courseCodesByUkprnResponse, HttpStatusCode.OK, null));


        reservationsClient.Setup(x => x.Get<GetAvailableDatesResponse>(It.IsAny<GetAvailableDatesRequest>()))
            .ReturnsAsync(availableDatesResponse);

        mapper.Setup(x => x.Map(learnersResponse.Data, It.IsAny<List<GetAllStandardsResponse.TrainingProgramme>>())).ReturnsAsync(learners);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Should().NotBeNull();
        actual.AccountLegalEntityId.Should().Be(query.AccountLegalEntityId);
        actual.EmployerName.Should().Be(aleResponse.LegalEntityName);
        actual.LastSubmissionDate.Should().Be(learnersResponse.LastSubmissionDate);
        actual.Page.Should().Be(learnersResponse.Page);
        actual.PageSize.Should().Be(learnersResponse.PageSize);
        actual.Total.Should().Be(learnersResponse.TotalItems);
        actual.TotalPages.Should().Be(learnersResponse.TotalPages);
        actual.Learners.Should().BeEquivalentTo(learners);
        actual.FutureMonths.Should().Be(3);
        actual.TrainingCourses.Count().Should().Be(courseCodesByUkprnResponse.CourseCodes.Count);
        actual.TrainingCourses.Select(t => t.Name).Should().BeEquivalentTo(courseCodesResponse.TrainingProgrammes.Select(t => t.Name));
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_With_LegalEntityId_For_LevyEmp_And_Clients_Return_Responses_The_result_Is_Returned_And_FutureMonths_Is_Zero(
       GetLearnersForProviderQuery query,
       GetLearnersForProviderRequest learnersRequest,
       GetLearnersForProviderResponse learnersResponse,
       GetAccountLegalEntityRequest aleRequest,
       GetAccountLegalEntityResponse aleResponse,
       GetAllStandardsRequest coursesRequest,
       GetAllStandardsResponse coursesResponse,
       GetCourseCodesResponse courseCodesResponse,
       GetCourseCodesByUkprnResponse courseCodesByUkprnResponse,
       List<LearnerSummary> learners,
       [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
       [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
       [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationsClient,
       [Frozen] Mock<IMapLearnerRecords> mapper,
       [Greedy] GetLearnersForProviderQueryHandler handler
   )
    {
        query.CohortId = null;
        GetLearnersForProviderRequest input;
        aleResponse.LevyStatus = ApprenticeshipEmployerType.Levy;

        learnerDataClient.Setup(x =>
                x.GetWithResponseCode<GetLearnersForProviderResponse>(It.IsAny<GetLearnersForProviderRequest>()))
            .ReturnsAsync(new ApiResponse<GetLearnersForProviderResponse>(learnersResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x =>
                x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
            .ReturnsAsync(new ApiResponse<GetAccountLegalEntityResponse>(aleResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x => x.GetWithResponseCode<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(coursesResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x =>
            x.GetWithResponseCode<GetCourseCodesResponse>(It.IsAny<GetAllTrainingProgrammesRequest>()))
           .ReturnsAsync(new ApiResponse<GetCourseCodesResponse>(courseCodesResponse, HttpStatusCode.OK, null));

        learnerDataClient.Setup(x => x.GetWithResponseCode<GetCourseCodesByUkprnResponse>(It.Is<GetCourseCodesByUkprnRequest>(t => t.Ukprn == query.ProviderId)))
            .ReturnsAsync(new ApiResponse<GetCourseCodesByUkprnResponse>(courseCodesByUkprnResponse, HttpStatusCode.OK, null));

        mapper.Setup(x => x.Map(learnersResponse.Data, It.IsAny<List<GetAllStandardsResponse.TrainingProgramme>>())).ReturnsAsync(learners);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Should().NotBeNull();
        actual.AccountLegalEntityId.Should().Be(query.AccountLegalEntityId);
        actual.EmployerName.Should().Be(aleResponse.LegalEntityName);
        actual.LastSubmissionDate.Should().Be(learnersResponse.LastSubmissionDate);
        actual.Page.Should().Be(learnersResponse.Page);
        actual.PageSize.Should().Be(learnersResponse.PageSize);
        actual.Total.Should().Be(learnersResponse.TotalItems);
        actual.TotalPages.Should().Be(learnersResponse.TotalPages);
        actual.Learners.Should().BeEquivalentTo(learners);
        actual.FutureMonths.Should().Be(0);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_With_CohortId_And_Clients_Return_Responses_The_result_Is_Returned(
    GetLearnersForProviderQuery query,
    GetLearnersForProviderRequest learnersRequest,
    GetLearnersForProviderResponse learnersResponse,
    GetCohortRequest cohortRequest,
    GetCohortResponse cohortResponse,
    GetAllStandardsRequest coursesRequest,
    GetAllStandardsResponse coursesResponse,
    GetCourseCodesResponse courseCodesResponse,
    GetCourseCodesByUkprnResponse getCourseCodesByUkprnResponse,
    GetDraftApprenticeshipsResponse apprenticeshipsResponse,
    List<LearnerSummary> learners,
    [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
    [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
    [Frozen] Mock<IMapLearnerRecords> mapper,
    [Greedy] GetLearnersForProviderQueryHandler handler
)
    {

        var trainingProgrammes = new List<TrainingProgramme>()
        {
             new TrainingProgramme() { CourseCode = "1" , Name = "course1" },
             new TrainingProgramme() { CourseCode = "2" , Name = "course2" },
             new TrainingProgramme() { CourseCode = "3" , Name = "course3" }
        };

        var courseCodes = new List<int>() { 1, 2 };
        getCourseCodesByUkprnResponse.CourseCodes = courseCodes;

        courseCodesResponse.TrainingProgrammes = trainingProgrammes;


        query.AccountLegalEntityId = null;
        GetLearnersForProviderRequest input;
        learnerDataClient.Setup(x =>
                x.GetWithResponseCode<GetLearnersForProviderResponse>(It.IsAny<GetLearnersForProviderRequest>()))
            .ReturnsAsync(new ApiResponse<GetLearnersForProviderResponse>(learnersResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x =>
                x.GetWithResponseCode<GetCohortResponse>(It.IsAny<GetCohortRequest>()))
            .ReturnsAsync(new ApiResponse<GetCohortResponse>(cohortResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x => x.GetWithResponseCode<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(coursesResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x => x.GetWithResponseCode<GetDraftApprenticeshipsResponse>(It.IsAny<GetDraftApprenticeshipsRequest>()))
            .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipsResponse>(apprenticeshipsResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x =>
          x.GetWithResponseCode<GetCourseCodesResponse>(It.IsAny<GetAllTrainingProgrammesRequest>()))
       .ReturnsAsync(new ApiResponse<GetCourseCodesResponse>(courseCodesResponse, HttpStatusCode.OK, null));

        learnerDataClient.Setup(x => x.GetWithResponseCode<GetCourseCodesByUkprnResponse>(It.Is<GetCourseCodesByUkprnRequest>(t => t.Ukprn == query.ProviderId)))
            .ReturnsAsync(new ApiResponse<GetCourseCodesByUkprnResponse>(getCourseCodesByUkprnResponse, HttpStatusCode.OK, null));

        mapper.Setup(x => x.Map(learnersResponse.Data, It.IsAny<List<GetAllStandardsResponse.TrainingProgramme>>())).ReturnsAsync(learners);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Should().NotBeNull();
        actual.AccountLegalEntityId.Should().Be(cohortResponse.AccountLegalEntityId);
        actual.EmployerName.Should().Be(cohortResponse.LegalEntityName);
        actual.LastSubmissionDate.Should().Be(learnersResponse.LastSubmissionDate);
        actual.Page.Should().Be(learnersResponse.Page);
        actual.PageSize.Should().Be(learnersResponse.PageSize);
        actual.Total.Should().Be(learnersResponse.TotalItems);
        actual.TotalPages.Should().Be(learnersResponse.TotalPages);
        actual.Learners.Should().BeEquivalentTo(learners);
        actual.TrainingCourses.Count.Should().Be(getCourseCodesByUkprnResponse.CourseCodes.Count);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_learnerData_Clients_fails(
        GetLearnersForProviderQuery query,
        GetLearnersForProviderRequest learnersRequest,
        GetLearnersForProviderResponse learnersResponse,
        GetAccountLegalEntityRequest aleRequest,
        GetAccountLegalEntityResponse aleResponse,
        GetAllStandardsRequest coursesRequest,
        GetAllStandardsResponse coursesResponse,
        List<LearnerSummary> learners,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IMapLearnerRecords> mapper,
        [Greedy] GetLearnersForProviderQueryHandler handler
    )
    {
        GetLearnersForProviderRequest input;
        query.CohortId = null;

        learnerDataClient.Setup(x =>
                x.GetWithResponseCode<GetLearnersForProviderResponse>(It.IsAny<GetLearnersForProviderRequest>()))
            .ReturnsAsync(new ApiResponse<GetLearnersForProviderResponse>(learnersResponse,
                HttpStatusCode.InternalServerError, "Call to learner data failed"));

        commitmentsClient.Setup(x =>
                x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
            .ReturnsAsync(new ApiResponse<GetAccountLegalEntityResponse>(aleResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x => x.GetWithResponseCode<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(coursesResponse, HttpStatusCode.OK, null));

        mapper.Setup(x => x.Map(learnersResponse.Data, It.IsAny<List<GetAllStandardsResponse.TrainingProgramme>>())).ReturnsAsync(learners);

        var result = async () => await handler.Handle(query, CancellationToken.None);

        await result.Should().ThrowAsync<ApplicationException>().WithMessage("*Call to learner data failed");
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_aleClients_fails(
        GetLearnersForProviderQuery query,
        GetLearnersForProviderRequest learnersRequest,
        GetLearnersForProviderResponse learnersResponse,
        GetAccountLegalEntityRequest aleRequest,
        GetAccountLegalEntityResponse aleResponse,
        GetAllStandardsRequest coursesRequest,
        GetAllStandardsResponse coursesResponse,
        List<LearnerSummary> learners,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IMapLearnerRecords> mapper,
        [Greedy] GetLearnersForProviderQueryHandler handler
    )
    {
        query.CohortId = null;
        learnerDataClient.Setup(x =>
                x.GetWithResponseCode<GetLearnersForProviderResponse>(It.IsAny<GetLearnersForProviderRequest>()))
            .ReturnsAsync(new ApiResponse<GetLearnersForProviderResponse>(learnersResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x =>
                x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
            .ReturnsAsync(new ApiResponse<GetAccountLegalEntityResponse>(aleResponse,
                HttpStatusCode.InternalServerError, "ALE failed"));

        commitmentsClient.Setup(x => x.GetWithResponseCode<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(coursesResponse, HttpStatusCode.OK, null));

        mapper.Setup(x => x.Map(learnersResponse.Data, It.IsAny<List<GetAllStandardsResponse.TrainingProgramme>>())).ReturnsAsync(learners);

        var result = async () => await handler.Handle(query, CancellationToken.None);

        await result.Should().ThrowAsync<ApplicationException>().WithMessage("*ALE failed");
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_cohortClient_fails(
        GetLearnersForProviderQuery query,
        GetLearnersForProviderRequest learnersRequest,
        GetLearnersForProviderResponse learnersResponse,
        GetCohortRequest cohortRequest,
        GetCohortResponse cohortResponse,
        GetAllStandardsRequest coursesRequest,
        GetAllStandardsResponse coursesResponse,
        List<LearnerSummary> learners,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IMapLearnerRecords> mapper,
        [Greedy] GetLearnersForProviderQueryHandler handler
    )
    {
        query.AccountLegalEntityId = null;
        learnerDataClient.Setup(x =>
                x.GetWithResponseCode<GetLearnersForProviderResponse>(It.IsAny<GetLearnersForProviderRequest>()))
            .ReturnsAsync(new ApiResponse<GetLearnersForProviderResponse>(learnersResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x =>
                x.GetWithResponseCode<GetCohortResponse>(It.IsAny<GetCohortRequest>()))
            .ReturnsAsync(new ApiResponse<GetCohortResponse>(cohortResponse,
                HttpStatusCode.InternalServerError, "Cohort failed"));

        commitmentsClient.Setup(x => x.GetWithResponseCode<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(coursesResponse, HttpStatusCode.OK, null));

        mapper.Setup(x => x.Map(learnersResponse.Data, It.IsAny<List<GetAllStandardsResponse.TrainingProgramme>>())).ReturnsAsync(learners);

        var result = async () => await handler.Handle(query, CancellationToken.None);

        await result.Should().ThrowAsync<ApplicationException>().WithMessage("*Cohort failed");
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_coursesClient_fails(
    GetLearnersForProviderQuery query,
    GetLearnersForProviderRequest learnersRequest,
    GetLearnersForProviderResponse learnersResponse,
    GetAccountLegalEntityRequest aleRequest,
    GetAccountLegalEntityResponse aleResponse,
    GetAllStandardsRequest coursesRequest,
    GetAllStandardsResponse coursesResponse,
    GetCohortResponse cohortResponse,
    List<LearnerSummary> learners,
    [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
    [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
    [Frozen] Mock<IMapLearnerRecords> mapper,
    [Greedy] GetLearnersForProviderQueryHandler handler
)
    {
        query.CohortId = null;
        learnerDataClient.Setup(x =>
                x.GetWithResponseCode<GetLearnersForProviderResponse>(It.IsAny<GetLearnersForProviderRequest>()))
            .ReturnsAsync(new ApiResponse<GetLearnersForProviderResponse>(learnersResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x =>
                x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
            .ReturnsAsync(new ApiResponse<GetAccountLegalEntityResponse>(aleResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x => x.GetWithResponseCode<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(coursesResponse, HttpStatusCode.OK, "Courses failed"));


        mapper.Setup(x => x.Map(learnersResponse.Data, It.IsAny<List<GetAllStandardsResponse.TrainingProgramme>>())).ReturnsAsync(learners);

        var result = async () => await handler.Handle(query, CancellationToken.None);

        await result.Should().ThrowAsync<ApplicationException>().WithMessage("*Courses failed");
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_Draft_Apprenticeships_Fails(
        GetLearnersForProviderQuery query,
        GetCohortRequest cohortRequest,
        GetCohortResponse cohortResponse,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Greedy] GetLearnersForProviderQueryHandler handler
    )
    {
        query.AccountLegalEntityId = null;

        commitmentsClient.Setup(x =>
           x.GetWithResponseCode<GetCohortResponse>(It.IsAny<GetCohortRequest>()))
       .ReturnsAsync(new ApiResponse<GetCohortResponse>(cohortResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x => x.GetWithResponseCode<GetDraftApprenticeshipsResponse>(It.IsAny<GetDraftApprenticeshipsRequest>()))
            .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipsResponse>(null, HttpStatusCode.InternalServerError, "Draft Apprenticeships Failed"));

        var result = async () => await handler.Handle(query, CancellationToken.None);

        await result.Should().ThrowAsync<ApplicationException>().WithMessage("*Draft Apprenticeships Failed");
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_Draft_Apprenticeships_Returs_Then_Data_Is_Included_In_LearnerApi_Call(
        GetLearnersForProviderQuery query,
        GetLearnersForProviderResponse learnersResponse,
        GetCourseCodesByUkprnResponse courseCodesByUkprnResponse,
        GetCohortResponse cohortResponse,
        GetCourseCodesResponse getCourseCodesResponse,
        GetDraftApprenticeshipsResponse draftApprenticeshipsResponse,
        GetAllStandardsResponse coursesResponse,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Greedy] GetLearnersForProviderQueryHandler handler
    )
    {
        query.AccountLegalEntityId = null;
        IGetApiRequest captured = null;
        var excludeUlns = string.Join(",", draftApprenticeshipsResponse.DraftApprenticeships.ConvertAll(x => x.Uln));

        learnerDataClient.Setup(x =>
                x.GetWithResponseCode<GetLearnersForProviderResponse>(It.IsAny<GetLearnersForProviderRequest>()))
                .Callback<IGetApiRequest>(r => captured = r)
                .ReturnsAsync(new ApiResponse<GetLearnersForProviderResponse>(learnersResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x =>
           x.GetWithResponseCode<GetCohortResponse>(It.IsAny<GetCohortRequest>()))
       .ReturnsAsync(new ApiResponse<GetCohortResponse>(cohortResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x =>
           x.GetWithResponseCode<GetCourseCodesResponse>(It.IsAny<GetAllTrainingProgrammesRequest>()))
        .ReturnsAsync(new ApiResponse<GetCourseCodesResponse>(getCourseCodesResponse, HttpStatusCode.OK, null));

        learnerDataClient.Setup(x => x.GetWithResponseCode<GetCourseCodesByUkprnResponse>(It.Is<GetCourseCodesByUkprnRequest>(t => t.Ukprn == query.ProviderId)))
            .ReturnsAsync(new ApiResponse<GetCourseCodesByUkprnResponse>(courseCodesByUkprnResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x => x.GetWithResponseCode<GetDraftApprenticeshipsResponse>(It.IsAny<GetDraftApprenticeshipsRequest>()))
            .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipsResponse>(draftApprenticeshipsResponse, HttpStatusCode.OK, null));

        commitmentsClient.Setup(x => x.GetWithResponseCode<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(coursesResponse, HttpStatusCode.OK, null));

        var result = await handler.Handle(query, CancellationToken.None);

        captured.Should().NotBeNull();
        captured.GetUrl.Should().Contain($"excludeUlns={excludeUlns}");
    }
}