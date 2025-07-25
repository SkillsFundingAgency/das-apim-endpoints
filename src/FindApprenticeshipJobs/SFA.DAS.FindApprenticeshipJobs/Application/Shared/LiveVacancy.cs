using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Shared
{
    public class LiveVacancy
    {
        public string Id { get; set; } = null!;
        public Guid VacancyId { get; set; }
        public string VacancyReference { get; set; }
        public string Title { get; set; } = null!;
        public int NumberOfPositions { get; set; }
        public string? ApprenticeshipTitle { get; set; }
        public string? Description { get; set; }
        public List<Address>? EmploymentLocations { get; set; }
        public string? EmploymentLocationInformation { get; set; }
        public AvailableWhere? EmploymentLocationOption { get; set; }
        public Address? Address { get; set; }
        public List<Address>? OtherAddresses { get; set; }
        public string? EmployerName { get; set; }
        public string ProviderName { get; set; } = null!;
        public long Ukprn { get; set; }
        public DateTime PostedDate { get; set; }
        public int StandardLarsCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public string? Route { get; set; }
        public int Level { get; set; }
        public Wage Wage { get; set; } = null!;
        public string? LongDescription { get; set; }
        public string? OutcomeDescription { get; set; }
        public string? TrainingDescription { get; set; }
        public IEnumerable<string> Skills { get; set; } = null!;
        public IEnumerable<Qualification> Qualifications { get; set; } = null!;
        public string? ThingsToConsider { get; set; }
        public string? AnonymousEmployerName { get; set; }
        public bool IsDisabilityConfident { get; set; }
        public bool IsEmployerAnonymous { get; set; }
        public bool IsPositiveAboutDisability { get; set; }
        public bool IsRecruitVacancy { get; set; }
        public string? VacancyLocationType { get; set; }
        public string? EmployerDescription { get; set; }
        public string? EmployerWebsiteUrl { get; set; }
        public string? EmployerContactPhone { get; set; }
        public string? EmployerContactEmail { get; set; }
        public string? EmployerContactName { get; set; }
        public string? ProviderContactEmail { get; set; }
        public string? ProviderContactName { get; set; }
        public string? ProviderContactPhone { get; set; }
        public string? ApprenticeshipLevel { get; set; }
        public int Duration { get; set; }
        public string? DurationUnit { get; set; }
        public int RouteCode { get; set; }
        public string AccountPublicHashedId { get; set; } = null!;
        public string AccountLegalEntityPublicHashedId { get; set; } = null!;
        public string ApplicationMethod { get; set; } = null!;
        public string? ApplicationInstructions { get; set; }
        public string? ApplicationUrl { get; set; }
        public string TypicalJobTitles { get; set; } = null!;
        public string? AdditionalQuestion1 { get; set; }
        public string? AdditionalQuestion2 { get; set; }
        public string? AdditionalTrainingDescription { get; set; }
        public string? SearchTags { get; set; }
        public ApprenticeshipTypes? ApprenticeshipType { get; set; }
    }
}
