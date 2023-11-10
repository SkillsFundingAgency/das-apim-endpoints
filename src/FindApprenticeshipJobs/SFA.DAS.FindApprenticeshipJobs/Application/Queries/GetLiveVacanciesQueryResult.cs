using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;
public class GetLiveVacanciesQueryResult
{
    public IEnumerable<LiveVacancy> Vacancies { get; set; } = null!;
    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public int TotalLiveVacanciesReturned { get; set; }
    public int TotalLiveVacancies { get; set; }
    public int TotalPages { get; set; }

    public class LiveVacancy
    {
        public Guid VacancyId { get; set; }
        public string VacancyTitle { get; set; }
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
        public int? RouteId { get; set; }
    }

    public class Address
    {
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? AddressLine4 { get; set; }
        public string? Postcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

