namespace SFA.DAS.FindApprenticeshipJobs.Application.Shared
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
    }
}
