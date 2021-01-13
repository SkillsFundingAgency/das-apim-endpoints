using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetTrainingProgrammeResponse
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public int MaxFunding { get; set; }

        public static implicit operator GetTrainingProgrammeResponse(GetStandardsListItem source)
        {
            return new GetTrainingProgrammeResponse
            {
                Id= source.Id,
                Duration = source.TypicalDuration,
                Level = source.Level,
                Title = source.Title,
                MaxFunding = source.MaxFunding
            };
        }
    }
}