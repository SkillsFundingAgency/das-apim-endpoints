using SFA.DAS.Campaign.Application.Queries.Standard;

namespace SFA.DAS.Campaign.Api.Models
{
    public class GetStandardByStandardUIdResponse
    {
        public string Title { get; set; }
        public int Level { get; set; }
        public int Duration { get; set; }
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public int MaxFunding { get; set; }

        public static implicit operator GetStandardByStandardUIdResponse(Standard s)
        {
            return new GetStandardByStandardUIdResponse
            {
                Title = s.Title,
                Level = s.Level,
                Duration = s.TimeToComplete,
                StandardUId = s.StandardUId,
                LarsCode = s.LarsCode,
                MaxFunding = s.MaxFunding
            };
        }
    }
}

