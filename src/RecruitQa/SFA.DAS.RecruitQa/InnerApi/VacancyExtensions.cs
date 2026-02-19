using SFA.DAS.RecruitQa.Domain;

namespace SFA.DAS.RecruitQa.InnerApi;

public static class VacancyExtensions
{
    public static Vacancy ToDomain(this VacancyDto source)
    {
        return new Vacancy
        {
            
            EmployerLocationOption = source.EmployerLocationOption,
            EmployerLocations = source.EmployerLocations,
            EmployerName = source.EmployerName,
            NumberOfPositions = source.NumberOfPositions,
            ProgrammeId = source.ProgrammeId,
            StartDate = source.StartDate,
            Title = source.Title,
            VacancyReference = source.VacancyReference,
            Wage = source.Wage,
        };
    }
}