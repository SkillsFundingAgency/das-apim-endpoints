using Microsoft.Azure.Amqp.Framing;
using SFA.DAS.Campaign.Application.Queries.Standard;
using System;

namespace SFA.DAS.Campaign.Api.Models
{
    public class GetStandardByLarsCodeResponse
    {
        public GetStandardResponse Standard { get; set; }
    }

    public class GetStandardResponse
    {
        public string Title { get; set; }
        public int Level { get; set; }
        public int Duration { get; set; }
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public int MaxFunding { get; set; }

        public static implicit operator GetStandardResponse(Standard s)
        {
            return new GetStandardResponse
            {
                Title = s.Title,
                Level = s.Level,
                Duration = s.Duration,
                StandardUId = s.StandardUId,
                LarsCode = s.LarsCode,
                MaxFunding = s.MaxFunding
            };
        }
    }
}

