using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Domain;

public class TrainingProgramme
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public static implicit operator TrainingProgramme(GetStandardsListItem source)
        {
            return new TrainingProgramme
            {
                Id =source.LarsCode.ToString(),
                Title = source.Title
            };
        }
    }