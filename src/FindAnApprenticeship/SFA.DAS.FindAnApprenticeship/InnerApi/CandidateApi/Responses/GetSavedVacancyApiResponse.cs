using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public record GetSavedVacancyApiResponse
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }
        public string VacancyReference { get; set; }
        public string? VacancyId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}