using System;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.Recruit.GraphQL.RecruitInner.Mappers;
using SFA.DAS.Recruit.InnerApi.Responses;
using SourceOrigin = SFA.DAS.Recruit.Domain.Vacancy.SourceOrigin;
using VacancyStatus = SFA.DAS.Recruit.Domain.Vacancy.VacancyStatus;
using ApprenticeshipTypes = SFA.DAS.SharedOuterApi.Domain.ApprenticeshipTypes;
using OwnerType = SFA.DAS.Recruit.Domain.Vacancy.OwnerType;
using ApplicationMethod = SFA.DAS.Recruit.Domain.Vacancy.ApplicationMethod;

namespace SFA.DAS.Recruit.Api.Models.Responses;

public record VacancyListItem(
    Guid Id,
    long? VacancyReference,
    string Title,
    string LegalEntityName,
    DateTime? ClosingDate,
    VacancyStatus Status,
    SourceOrigin? SourceOrigin,
    ApprenticeshipTypes? ApprenticeshipType,
    OwnerType? OwnerType,
    ApplicationMethod? ApplicationMethod,
    bool? HasSubmittedAdditionalQuestions,
    VacancyStatsItem Stats)
{
    public static VacancyListItem From(IGetPagedVacanciesList_PagedVacancies_Items source, VacancyStatsItem stats)
    {
        return new VacancyListItem(
            source.Id,
            source.VacancyReference,
            source.Title,
            source.LegalEntityName,
            source.ClosingDate?.UtcDateTime,
            source.Status.FromQueryType(),
            source.SourceOrigin.FromQueryType(),
            source.ApprenticeshipType.FromQueryType(),
            source.OwnerType.FromQueryType(),
            source.ApplicationMethod.FromQueryType(),
            source.HasSubmittedAdditionalQuestions,
            stats
        );
    }
}