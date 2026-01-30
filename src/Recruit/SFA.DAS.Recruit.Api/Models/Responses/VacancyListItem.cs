using System;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.Recruit.GraphQL.RecruitInner.Mappers;
using SFA.DAS.Recruit.InnerApi.Responses;
using SourceOrigin = SFA.DAS.Recruit.Domain.Vacancy.SourceOrigin;
using VacancyStatus = SFA.DAS.Recruit.Domain.Vacancy.VacancyStatus;

namespace SFA.DAS.Recruit.Api.Models.Responses;

public record VacancyListItem(Guid Id, long? VacancyReference, string Title, string EmployerName, DateTime? ClosingDate, VacancyStatus Status, SourceOrigin? SourceOrigin, VacancyStatsItem Stats)
{
    public static VacancyListItem From(IGetDashboardVacanciesPagedList_PagedVacancies_Items source, VacancyStatsItem stats)
    {
        return new VacancyListItem(
            source.Id,
            source.VacancyReference,
            source.Title,
            source.EmployerName,
            source.ClosingDate?.UtcDateTime,
            source.Status.FromQueryType(),
            source.SourceOrigin.FromQueryType(),
            stats
        );
    }
}