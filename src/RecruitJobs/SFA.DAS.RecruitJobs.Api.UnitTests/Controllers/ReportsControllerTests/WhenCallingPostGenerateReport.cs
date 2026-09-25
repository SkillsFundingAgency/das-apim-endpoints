using System.Collections.Generic;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.RecruitJobs.Domain.Reports;
using SFA.DAS.RecruitJobs.InnerApi.Requests.Reports;
using SFA.DAS.RecruitJobs.InnerApi.Responses.Reports;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.ReportsControllerTests;

public class WhenCallingPostGenerateReport
{
    [Test, MoqAutoData]
    public async Task Then_Enriched_Report_Is_Uploaded_When_Data_Exists(
        Guid id,
        Guid applicationId,
        Guid candidateId,
        long vacancyReference,
        StandardDetailResponse course,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<ICourseService> courseService,
        [Greedy] ReportsController controller)
    {
        var review = new ApplicationReviewReport
        {
            ApplicationId = applicationId,
            CandidateId = candidateId,
            VacancyReference = vacancyReference,
            ProgrammeId = 1,
            VacancyTitle = "Test Vacancy"
        };

        var baseData = new GetApplicationReviewReportResponse
        {
            ApplicationReviewReports = [review]
        };

        var application = new ReportApplication
        {
            Id = applicationId,
            CandidateId = candidateId,
            Candidate = new ReportCandidate { FirstName = "Jane", LastName = "Doe", Email = "jane@test.com" },
            Support = "None"
        };

        recruitApiClient
            .Setup(x => x.PostWithResponseCode<GetApplicationReviewReportResponse>(
                It.IsAny<PostReportsGenerateByReportIdApiRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<GetApplicationReviewReportResponse>(baseData, HttpStatusCode.OK, null!));

        candidateApiClient
            .Setup(x => x.Get<GetApplicationsByVacancyReferenceApiResponse>(
                It.IsAny<GetApplicationsByVacancyReferenceApiRequest>()))
            .ReturnsAsync(new GetApplicationsByVacancyReferenceApiResponse { Applications = [application] });

        courseService
            .Setup(x => x.GetStandardDetailsById(It.IsAny<string>()))
            .ReturnsAsync(course);

        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostReportsGenerateByReportIdUploadApiRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, null!));

        var result = await controller.PostGenerateReport(recruitApiClient.Object, candidateApiClient.Object, courseService.Object, id, CancellationToken.None);

        result.Should().BeOfType<Created>();
        recruitApiClient.Verify(x => x.PostWithResponseCode<NullResponse>(
            It.Is<PostReportsGenerateByReportIdUploadApiRequest>(r => r.ReportId == id), It.IsAny<bool>()), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Then_Empty_Result_Is_Uploaded_When_No_Reviews(
        Guid id,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<ICourseService> courseService,
        [Greedy] ReportsController controller)
    {
        var baseData = new GetApplicationReviewReportResponse { ApplicationReviewReports = [] };

        recruitApiClient
            .Setup(x => x.PostWithResponseCode<GetApplicationReviewReportResponse>(
                It.IsAny<PostReportsGenerateByReportIdApiRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<GetApplicationReviewReportResponse>(baseData, HttpStatusCode.OK, null!));

        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostReportsGenerateByReportIdUploadApiRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, null!));

        var result = await controller.PostGenerateReport(recruitApiClient.Object, candidateApiClient.Object, courseService.Object, id, CancellationToken.None);

        result.Should().BeOfType<Created>();
        recruitApiClient.Verify(x => x.PostWithResponseCode<NullResponse>(
            It.Is<PostReportsGenerateByReportIdUploadApiRequest>(r =>
                r.ReportId == id &&
                ((PostUploadApplicationSummaryReportRequest)r.Data).Reports.Count == 0),
            It.IsAny<bool>()), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Then_Problem_Is_Returned_When_Base_Data_Fetch_Fails(
        Guid id,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<ICourseService> courseService,
        [Greedy] ReportsController controller)
    {
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<GetApplicationReviewReportResponse>(
                It.IsAny<PostReportsGenerateByReportIdApiRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<GetApplicationReviewReportResponse>(null!, HttpStatusCode.InternalServerError, null!));

        var result = await controller.PostGenerateReport(recruitApiClient.Object, candidateApiClient.Object, courseService.Object, id, CancellationToken.None);

        result.Should().BeOfType<ProblemHttpResult>();
    }

    [Test, MoqAutoData]
    public async Task Then_Problem_Is_Returned_When_Upload_Fails(
        Guid id,
        Guid applicationId,
        Guid candidateId,
        long vacancyReference,
        StandardDetailResponse course,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<ICourseService> courseService,
        [Greedy] ReportsController controller)
    {
        var review = new ApplicationReviewReport
        {
            ApplicationId = applicationId,
            CandidateId = candidateId,
            VacancyReference = vacancyReference,
            ProgrammeId = 1
        };

        var baseData = new GetApplicationReviewReportResponse
        {
            ApplicationReviewReports = [review]
        };

        var application = new ReportApplication
        {
            Id = applicationId,
            CandidateId = candidateId,
            Candidate = new ReportCandidate { FirstName = "Jane", LastName = "Doe" }
        };

        recruitApiClient
            .Setup(x => x.PostWithResponseCode<GetApplicationReviewReportResponse>(
                It.IsAny<PostReportsGenerateByReportIdApiRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<GetApplicationReviewReportResponse>(baseData, HttpStatusCode.OK, null!));

        candidateApiClient
            .Setup(x => x.Get<GetApplicationsByVacancyReferenceApiResponse>(
                It.IsAny<GetApplicationsByVacancyReferenceApiRequest>()))
            .ReturnsAsync(new GetApplicationsByVacancyReferenceApiResponse { Applications = [application] });

        courseService
            .Setup(x => x.GetStandardDetailsById(It.IsAny<string>()))
            .ReturnsAsync(course);

        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostReportsGenerateByReportIdUploadApiRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.InternalServerError, null!));

        var result = await controller.PostGenerateReport(recruitApiClient.Object, candidateApiClient.Object, courseService.Object, id, CancellationToken.None);

        result.Should().BeOfType<ProblemHttpResult>();
    }
}
