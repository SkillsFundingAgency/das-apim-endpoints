namespace SFA.DAS.FindAnApprenticeship.Api.Models.Vacancies
{
    public record SaveVacancyApiRequest
    {
        public required string VacancyReference { get; set; }
    }
}