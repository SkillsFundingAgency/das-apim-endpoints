namespace SFA.DAS.FindAnApprenticeship.Api.Models.Vacancies
{
    public record DeleteSavedVacancyApiRequest
    {
        public string VacancyId { get; set; }
    }
}