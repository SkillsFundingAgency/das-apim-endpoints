using MediatR;
using SFA.DAS.Recruit.Domain.Reports;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses.Reports;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.Recruit.InnerApi.Recruit.Responses.Reports.GetGenerateReportResponse;

namespace SFA.DAS.Recruit.Application.Report.Query.GenerateReportsById;
public class GenerateReportsByReportIdQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    : IRequestHandler<GenerateReportsByReportIdQuery, GenerateReportsByReportIdQueryResult>
{
    public async Task<GenerateReportsByReportIdQueryResult> Handle(
    GenerateReportsByReportIdQuery request,
    CancellationToken cancellationToken)
    {
        var applicationSummaryReports = new List<ApplicationSummaryReport>();

        // Get report info from Recruit API
        var recruitResponse = await recruitApiClient
            .Get<GetGenerateReportResponse>(new GetGenerateReportRequest(request.ReportId));

        if (recruitResponse?.ApplicationReviewReports == null || recruitResponse.ApplicationReviewReports.Count == 0)
        {
            return new GenerateReportsByReportIdQueryResult { Reports = applicationSummaryReports };
        }

        // Collect unique vacancy references
        var vacancyReferences = recruitResponse.ApplicationReviewReports
            .Select(r => r.VacancyReference)
            .Distinct()
            .ToList();

        // Fetch all applications (including candidate details) for each vacancy
        var allApplications = new List<Domain.Application>();
        foreach (var vacancyRef in vacancyReferences)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var candidateApiResponse = await candidateApiClient.Get<GetApplicationsByVacancyReferenceApiResponse>(
                new GetApplicationsByVacancyReferenceApiRequest(vacancyRef));

            if (candidateApiResponse?.Applications?.Count > 0)
            {
                allApplications.AddRange(candidateApiResponse.Applications);
            }
        }

        var appLookup = allApplications
            .GroupBy(a => (a.Id, a.CandidateId))
            .ToDictionary(g => g.Key, g => g.First());

        var courseCache = new Dictionary<string, CourseInfo>(StringComparer.OrdinalIgnoreCase);

        // Process each review sequentially
        foreach (var review in recruitResponse.ApplicationReviewReports)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!appLookup.TryGetValue((review.ApplicationId, review.CandidateId), out var applicationResponse))
            {
                // If the application doesn't exist in the bulk response, skip it.
                continue;
            }

            if (!courseCache.TryGetValue(review.ProgrammeId.ToString(), out var courseInfo))
            {
                var courseResult = await coursesApiClient.Get<GetStandardsListItemResponse>(
                    new GetStandardRequest(review.ProgrammeId));
                if (courseResult == null)
                    continue;

                courseInfo = new CourseInfo(courseResult.Title, courseResult.Level, courseResult.Status);
                courseCache[review.ProgrammeId.ToString()] = courseInfo;
            }

            if (applicationResponse.Candidate == null)
                continue;

            var applicantData = MapBaseApplicantData(review);
            EnrichApplicantDataWithCandidate(applicantData, applicationResponse, courseInfo);

            applicationSummaryReports.Add(applicantData);
        }

        return new GenerateReportsByReportIdQueryResult
        {
            Reports = applicationSummaryReports
        };
    }
    
    private static ApplicationSummaryReport MapBaseApplicantData(ApplicationReviewReport review)
    {
        return new ApplicationSummaryReport
        {
            ApplicationId = review.ApplicationId,
            VacancyReferenceNumber = review.VacancyReference,
            VacancyTitle = review.VacancyTitle,
            Employer = review.EmployerName,
            LearningProvider = review.TrainingProviderName,
            ApplicationStatus = review.ApplicationStatus,
            NumberOfDaysApplicationAtThisStatus = review.NumberOfDaysApplicationAtThisStatus ?? 0,
            RecruitingNationally = review.AvailableWhere == AvailableWhere.AcrossEngland,
            ApplicationDate = review.ApplicationSubmittedDate,
            VacancyClosingDate = review.VacancyClosingDate,
            ApprenticeshipType = review.ApprenticeshipType
        };
    }

    private static void EnrichApplicantDataWithCandidate(
        ApplicationSummaryReport applicant,
        Domain.Application applicationResponse,
        CourseInfo courseInfo)
    {
        var candidate = applicationResponse.Candidate;
        if (candidate != null)
        {
            applicant.CandidateName = $"{candidate.FirstName} {candidate.LastName}".Trim();
            applicant.AddressLine1 = candidate.Address?.AddressLine1;
            applicant.AddressLine2 = candidate.Address?.AddressLine2;
            applicant.Town = candidate.Address?.Town;
            applicant.County = candidate.Address?.County;
            applicant.Postcode = candidate.Address?.Postcode;
            applicant.Email = candidate.Email;
            applicant.DateOfBirth = candidate.DateOfBirth?.ToString("dd/MM/yyyy") ?? string.Empty;
        }

        applicant.InterviewAssistance = applicationResponse.Support;
        applicant.CourseName = courseInfo?.Title ?? string.Empty;
        applicant.ApprenticeshipLevel = courseInfo?.Level ?? 0;
        applicant.CourseStatus = courseInfo?.Status ?? string.Empty;

        var addresses = applicationResponse.EmploymentLocation?.Addresses;
        if (addresses == null || addresses.Count == 0)
            return;

        var selectedAddresses = addresses
            .Where(x => x.IsSelected && x.AddressOrder > 0)
            .OrderBy(x => x.AddressOrder)
            .ToList();

        if (selectedAddresses.Count == 0 && addresses.Count == 1)
        {
            applicant.Workplace1 = addresses.First().Address.ToSingleLineAddress();
            return;
        }

        foreach (var location in selectedAddresses)
        {
            var workplaceNumber = location.AddressOrder;

            if (workplaceNumber is < 1 or > 10)
                continue;

            var addressString = location.Address?.ToSingleLineAddress();
            if (string.IsNullOrWhiteSpace(addressString))
                continue;

            typeof(ApplicationSummaryReport)
                .GetProperty($"Workplace{workplaceNumber}")
                ?.SetValue(applicant, addressString);
        }
    }
}