﻿namespace SFA.DAS.FindApprenticeshipJobs.Application.Shared
{
    public class LiveVacancy
    {
        public Guid VacancyId { get; set; }
        public long VacancyReference { get; set; }
        public string VacancyTitle { get; set; }
        public int NumberOfPositions { get; set; }
        public string ApprenticeshipTitle { get; set; }
        public string? Description { get; set; }
        public Address? EmployerLocation { get; set; }
        public string? EmployerName { get; set; }
        public string? ProviderName { get; set; }
        public long? ProviderId { get; set; }
        public DateTime LiveDate { get; set; }
        public string? ProgrammeId { get; set; }
        public string? ProgrammeType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public string Route { get; set; }
        public int Level { get; set; }
        public Wage? Wage { get; set; } = null!;
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
}
