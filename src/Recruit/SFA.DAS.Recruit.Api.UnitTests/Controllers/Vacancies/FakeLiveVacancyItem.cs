using System;
using SFA.DAS.Recruit.GraphQL;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Vacancies;

public class FakeLiveVacancyItem : IGetPagedVacanciesList_PagedVacancies_Items
{
    public Guid Id { get; set; }
    public long? VacancyReference { get; set; }// = 123456;
    public string? Title { get; set; }
    public string? LegalEntityName { get; set; }
    public DateTimeOffset? ClosingDate { get; set; }
    public VacancyStatus Status => VacancyStatus.Live;
    public SourceOrigin? SourceOrigin { get; set; }
    public ApprenticeshipTypes? ApprenticeshipType { get; set; }
    public OwnerType? OwnerType { get; set; }
    public ApplicationMethod? ApplicationMethod { get; set; }
    public bool? HasSubmittedAdditionalQuestions { get; set; }
    public string? TransferInfo { get; set; }
}