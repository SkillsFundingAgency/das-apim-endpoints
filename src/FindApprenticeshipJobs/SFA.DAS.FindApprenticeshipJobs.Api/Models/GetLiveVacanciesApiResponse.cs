using SFA.DAS.FindApprenticeshipJobs.Application.Queries;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Models;

public class GetLiveVacanciesApiResponse
{
    public static implicit operator GetLiveVacanciesApiResponse(GetLiveVacanciesQueryResult source)
    {
        return new GetLiveVacanciesApiResponse
        {
            Vacancies = source.Vacancies.Select(x => (LiveVacancy)x),
            PageSize = source.PageSize,
            PageNo = source.PageNo,
            TotalLiveVacanciesReturned = source.TotalLiveVacanciesReturned,
            TotalLiveVacancies = source.TotalLiveVacancies,
            TotalPages = source.TotalPages
        };
    }

    public IEnumerable<LiveVacancy> Vacancies { get; set; } = null!;
    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public int TotalLiveVacanciesReturned { get; set; }
    public int TotalLiveVacancies { get; set; }
    public int TotalPages { get; set; }

    public class LiveVacancy
    {
        public static implicit operator LiveVacancy(GetLiveVacanciesQueryResult.LiveVacancy source)
        {
            return new LiveVacancy
            {
                VacancyId = source.VacancyId,
                VacancyTitle = source.VacancyTitle,
                ApprenticeshipTitle = source.ApprenticeshipTitle,
                ProgrammeId = source.ProgrammeId,
                ProgrammeType = source.ProgrammeType,
                RouteId = source.RouteId,
                Description = source.Description,
                EmployerLocation = source.EmployerLocation == null? null : (Address) source.EmployerLocation,
                LiveDate = source.LiveDate,
                StartDate = source.StartDate,
                EmployerName = source.EmployerName,
            };
        }

        public Guid VacancyId { get; set; }
        public string VacancyTitle { get; set; }
        public string ApprenticeshipTitle { get; set; }
        public string? Description { get; set; }
        public Address? EmployerLocation { get; set; }
        public string? EmployerName { get; set; }
        public DateTime LiveDate { get; set; }
        public string? ProgrammeId { get; set; }
        public string? ProgrammeType { get; set; }
        public DateTime StartDate { get; set; }
        public int? RouteId { get; set; }
    }

    public class Address
    {
        public static implicit operator Address(GetLiveVacanciesQueryResult.Address source)
        {
            return new Address
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                AddressLine4 = source.AddressLine4,
                Postcode = source.Postcode,
                Latitude = source.Latitude,
                Longitude = source.Longitude
            };
        }

        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? AddressLine4 { get; set; }
        public string? Postcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
