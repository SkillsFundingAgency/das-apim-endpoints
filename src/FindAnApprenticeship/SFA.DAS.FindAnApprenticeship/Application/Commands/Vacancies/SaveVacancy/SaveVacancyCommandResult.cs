using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies.SaveVacancy
{
    public record SaveVacancyCommandResult
    {
        public Guid Id { get; set; }

        public static implicit operator SaveVacancyCommandResult(PutSavedVacancyApiResponse source)
        {
            return new SaveVacancyCommandResult
            {
                Id = source.Id
            };
        }
    }
}