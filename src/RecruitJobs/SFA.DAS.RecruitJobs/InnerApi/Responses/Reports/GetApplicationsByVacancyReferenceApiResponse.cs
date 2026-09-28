using System.Collections.Generic;
using SFA.DAS.RecruitJobs.Domain.Reports;

namespace SFA.DAS.RecruitJobs.InnerApi.Responses.Reports;

public record GetApplicationsByVacancyReferenceApiResponse
{
    public List<ReportApplication> Applications { get; init; } = [];
}
