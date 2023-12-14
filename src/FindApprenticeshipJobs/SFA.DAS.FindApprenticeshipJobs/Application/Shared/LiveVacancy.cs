namespace SFA.DAS.FindApprenticeshipJobs.Application.Shared
{
    public class LiveVacancy
    {
        public Guid VacancyId { get; set; }
        public long VacancyReference { get; set; }
        public string Title { get; set; } = string.Empty;
        public int NumberOfPositions { get; set; }
        public string ApprenticeshipTitle { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Address? Address { get; set; }
        public string? EmployerName { get; set; }
        public string? ProviderName { get; set; }
        public long Ukprn { get; set; }
        public DateTime PostedDate { get; set; }
        public int StandardLarsCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public string Route { get; set; } = string.Empty;
        public int Level { get; set; }
        public Wage? Wage { get; set; } = null!;
        public string LongDescription { get; set; } = string.Empty;
        public string OutcomeDescription { get; set; } = string.Empty;
        public string TrainingDescription { get; set; } = string.Empty;
        public IEnumerable<string> Skills { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<Qualification> Qualifications { get; set; } = Enumerable.Empty<Qualification>();
        public string ThingsToConsider { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string AnonymousEmployerName { get; set; } = string.Empty;
        public bool IsDisabilityConfident { get; set; }
        public bool IsEmployerAnonymous { get; set; }
        public bool IsPositiveAboutDisability { get; set; }
        public bool IsRecruitVacancy { get; set; }
        public string VacancyLocationType { get; set; } = string.Empty;
        public string EmployerDescription { get; set; } = string.Empty;
        public string EmployerWebsiteUrl { get; set; } = string.Empty;
        public string EmployerContactPhone { get; set; } = string.Empty;
        public string EmployerContactEmail { get; set; } = string.Empty;
        public string EmployerContactName { get; set; } = string.Empty;
        public string ApprenticeshipLevel { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string? DurationUnit { get; set; }
        public int RouteCode { get; set; }
        public string? AccountPublicHashedId { get; set; }
        public string? AccountLegalEntityPublicHashedId { get; set; }
    }
}
