using MediatR;
using SFA.DAS.Recruit.Domain.Reports;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Requests.Reports;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.Recruit.InnerApi.Requests.Reports.GetGenerateReportResponse;

namespace SFA.DAS.Recruit.Application.Report.Query.GenerateReportsById;
public class GenerateReportsByReportIdQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    : IRequestHandler<GenerateReportsByReportIdQuery, GenerateReportsByReportIdQueryResult>
{
    public async Task<GenerateReportsByReportIdQueryResult> Handle(GenerateReportsByReportIdQuery request,
        CancellationToken cancellationToken)
    {
        var applicationSummaryReports = new List<ApplicationSummaryReport>();

        var recruitResponse = await recruitApiClient
            .Get<GetGenerateReportResponse>(new GetGenerateReportRequest(request.ReportId));

        if (recruitResponse?.ApplicationReviewReports == null || recruitResponse.ApplicationReviewReports.Count == 0)
        {
            return new GenerateReportsByReportIdQueryResult { Report = applicationSummaryReports };
        }

        foreach (var review in recruitResponse.ApplicationReviewReports)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var applicantData = MapBaseApplicantData(review);

            // Get application details
            var applicationResponse = await candidateApiClient.Get<Domain.Application>(
                new GetApplicationByIdApiRequest(review.ApplicationId, review.CandidateId));

            // Get course info
            var courseResult = await coursesApiClient.Get<GetStandardsListItemResponse>(
                new GetStandardRequest(review.ProgrammeId));

            // Skip incomplete data
            if (applicationResponse?.Candidate == null)
            {
                continue;
            }

            // Fill candidate and course details
            EnrichApplicantDataWithCandidate(applicantData, applicationResponse, courseResult);

            applicationSummaryReports.Add(applicantData);
        }

        return new GenerateReportsByReportIdQueryResult
        {
            Report = applicationSummaryReports
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
        GetStandardsListItemResponse course)
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
        applicant.CourseName = course?.Title ?? string.Empty;
        applicant.ApprenticeshipLevel = course?.Level ?? 0;
        applicant.CourseStatus = course?.Status ?? string.Empty;

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