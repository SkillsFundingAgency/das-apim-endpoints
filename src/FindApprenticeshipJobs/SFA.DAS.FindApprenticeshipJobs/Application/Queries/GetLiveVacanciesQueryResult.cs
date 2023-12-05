using SFA.DAS.FindApprenticeshipJobs.Application.Shared;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;

public class GetLiveVacanciesQueryResult
{
    public IEnumerable<LiveVacancy> Vacancies { get; set; } = null!;
    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public int TotalLiveVacanciesReturned { get; set; }
    public int TotalLiveVacancies { get; set; }
    public int TotalPages { get; set; }
        public string LongDescription { get; set; }
        public string OutcomeDescription { get; set; }
        public string TrainingDescription { get; set; }
        public IEnumerable<string> Skills { get; set; }
        public IEnumerable<Qualification> Qualifications { get; set; }
        public string ThingsToConsider { get; set; }
        public string Id { get; set; }
        public string AnonymousEmployerName { get; set; }
        public string Category { get; set; }
        public string CategoryCode { get; set; }
        public DisabilityConfident IsDisabilityConfident { get; set; }
        public bool IsEmployerAnonymous { get; set; }
        public bool IsPositiveAboutDisability { get; set; }
        public bool IsRecruitVacancy { get; set; }
        public string SubCategory { get; set; }
        public string SubCategoryCode { get; set; }
        public string VacancyLocationType { get; set; }
        public long WageAmountLowerBand { get; set; }
        public long WageAmountUpperBand { get; set; }
        public int ExpectedDuration { get; set; }
        public int Distance { get; set; }
        public int Score { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerWebsiteUrl { get; set; }
        public string EmployerContactPhone { get; set; }
        public string EmployerContactEmail { get; set; }
        public string EmployerContactName { get; set; }
    }

    public class Qualification
    {
        public string? QualificationType { get; set; }
        public string? Subject { get; set; }
        public string? Grade { get; set; }
        public string? Weighting { get; set; }
    }

    public enum DisabilityConfident
    {
        No = 0,
        Yes
}

