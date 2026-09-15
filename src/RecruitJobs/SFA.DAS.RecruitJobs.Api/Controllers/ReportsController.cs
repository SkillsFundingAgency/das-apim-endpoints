using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitJobs.Domain.Reports;
using SFA.DAS.RecruitJobs.InnerApi.Requests.Reports;
using SFA.DAS.RecruitJobs.InnerApi.Responses.Reports;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ReportsController(ILogger<ReportsController> logger) : ControllerBase
{
    [HttpPost]
    [Route("generate/{id:guid}")]
    public async Task<IResult> PostGenerateReport(
        [FromServices] Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration> recruitApiClient,
        [FromServices] ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
        [FromServices] ICourseService courseService,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("RecruitJobs: Generating enriched report for report Id: {ReportId}", id);

        var baseResponse = await recruitApiClient.PostWithResponseCode<GetApplicationReviewReportResponse>(
            new PostReportsGenerateByReportIdApiRequest { ReportId = id });

        if (baseResponse.StatusCode != HttpStatusCode.OK)
        {
            logger.LogWarning("RecruitJobs: Failed to retrieve base report data for {ReportId} — status {StatusCode}", id, baseResponse.StatusCode);
            return TypedResults.Problem();
        }

        var reviews = baseResponse.Body?.ApplicationReviewReports;

        if (reviews == null || reviews.Count == 0)
        {
            logger.LogInformation("RecruitJobs: No application reviews found for report {ReportId}", id);
            return TypedResults.Created();
        }

        var vacancyReferences = reviews.Select(r => r.VacancyReference).Distinct().ToList();
        var allApplications = new ConcurrentBag<ReportApplication>();

        await Parallel.ForEachAsync(vacancyReferences, new ParallelOptions
        {
            MaxDegreeOfParallelism = 10,
            CancellationToken = cancellationToken
        }, async (vacancyRef, ct) =>
        {
            var response = await candidateApiClient.Get<GetApplicationsByVacancyReferenceApiResponse>(
                new GetApplicationsByVacancyReferenceApiRequest(vacancyRef));

            if (response?.Applications.Count > 0)
                foreach (var app in response.Applications)
                    allApplications.Add(app);
        });

        var appLookup = allApplications
            .GroupBy(a => (a.Id, a.CandidateId))
            .ToDictionary(g => g.Key, g => g.First());

        // Step 3: enrich each review with candidate + course data
        var courseCache = new Dictionary<string, SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses.StandardDetailResponse>(StringComparer.OrdinalIgnoreCase);
        var summaryReports = new List<ApplicationSummaryReport>();

        foreach (var review in reviews)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!appLookup.TryGetValue((review.ApplicationId, review.CandidateId), out var application))
                continue;

            if (application.Candidate == null)
                continue;

            var programmeKey = review.ProgrammeId.ToString();
            if (!courseCache.TryGetValue(programmeKey, out var course))
            {
                course = await courseService.GetStandardDetailsById(programmeKey);
                if (course == null)
                {
                    logger.LogWarning("RecruitJobs: Course not found for programme {ProgrammeId} — skipping application {ApplicationId}", review.ProgrammeId, review.ApplicationId);
                    continue;
                }
                courseCache[programmeKey] = course;
            }

            summaryReports.Add(BuildSummaryReport(review, application, course));
        }

        // Step 4: upload enriched data back to Recruit API
        var uploadRequest = new PostUploadApplicationSummaryReportRequest { Reports = summaryReports };
        var uploadResponse = await recruitApiClient.PostWithResponseCode<NullResponse>(
            new PostReportsGenerateByReportIdUploadApiRequest(uploadRequest) { ReportId = id });

        if (!uploadResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("RecruitJobs: Failed to upload enriched report for {ReportId} — status {StatusCode}", id, uploadResponse.StatusCode);
            return TypedResults.Problem();
        }

        logger.LogInformation("RecruitJobs: Successfully generated and uploaded {Count} records for report {ReportId}", summaryReports.Count, id);
        return TypedResults.Created();
    }

    private static ApplicationSummaryReport BuildSummaryReport(
        ApplicationReviewReport review,
        ReportApplication application,
        SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses.StandardDetailResponse course)
    {
        var candidate = application.Candidate!;

        var report = new ApplicationSummaryReport
        {
            CandidateId = review.CandidateId,
            CandidateName = $"{candidate.FirstName} {candidate.LastName}".Trim(),
            AddressLine1 = candidate.Address?.AddressLine1 ?? "",
            AddressLine2 = candidate.Address?.AddressLine2 ?? "",
            Town = candidate.Address?.Town ?? "",
            County = candidate.Address?.County ?? "",
            Postcode = candidate.Address?.Postcode ?? "",
            Email = candidate.Email ?? "",
            Telephone = candidate.PhoneNumber ?? "",
            DateOfBirth = candidate.DateOfBirth?.ToString("dd/MM/yyyy") ?? "",
            VacancyReferenceNumber = review.VacancyReference,
            VacancyTitle = review.VacancyTitle ?? "",
            Employer = review.EmployerName ?? "",
            LearningProvider = review.TrainingProviderName ?? "",
            ApplicationStatus = review.ApplicationStatus,
            NumberOfDaysApplicationAtThisStatus = review.NumberOfDaysApplicationAtThisStatus ?? 0,
            RecruitingNationally = review.AvailableWhere == AvailableWhere.AcrossEngland,
            ApplicationDate = review.ApplicationSubmittedDate,
            VacancyClosingDate = review.VacancyClosingDate,
            ApprenticeshipType = review.ApprenticeshipType,
            InterviewAssistance = application.Support ?? "",
            CourseName = course.Title ?? "",
            CourseId = course.LarsCode,
            ApprenticeshipLevel = course.Level,
            CourseStatus = course.Status ?? "",
        };

        EnrichWorkplaceAddresses(report, application.EmploymentLocation);

        return report;
    }

    private static void EnrichWorkplaceAddresses(ApplicationSummaryReport report, ReportEmploymentLocation? employmentLocation)
    {
        var addresses = employmentLocation?.Addresses;
        if (addresses == null || addresses.Count == 0)
            return;

        var selected = addresses
            .Where(x => x.IsSelected && x.AddressOrder > 0)
            .OrderBy(x => x.AddressOrder)
            .ToList();

        if (selected.Count == 0 && addresses.Count == 1)
        {
            report.Workplace1 = addresses[0].GetAddress().ToSingleLineAddress() ?? "";
            report.VacancyPostcode = addresses[0].GetAddress()?.Postcode ?? "";
            return;
        }

        foreach (var location in selected)
        {
            var order = location.AddressOrder;
            if (order is < 1 or > 10)
                continue;

            var addressLine = location.GetAddress()?.ToSingleLineAddress();
            if (string.IsNullOrWhiteSpace(addressLine))
                continue;

            typeof(ApplicationSummaryReport)
                .GetProperty($"Workplace{order}")
                ?.SetValue(report, addressLine);
        }

        report.VacancyPostcode = addresses[0].GetAddress()?.Postcode ?? "";
    }
}
