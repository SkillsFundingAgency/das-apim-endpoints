using SFA.DAS.ManageApprenticeships.InnerApi.Responses;

namespace SFA.DAS.ManageApprenticeships.Api.Models
{
    public class GetStandardResponse
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public int MaxFunding { get; set; }

        public static implicit operator GetStandardResponse(GetStandardsListItem source)
        {
            return new GetStandardResponse
            {
                Id= source.LarsCode,
                Duration = source.TypicalDuration,
                Level = source.Level,
                Title = source.Title,
                MaxFunding = source.MaxFunding
            };
        }
    }
}