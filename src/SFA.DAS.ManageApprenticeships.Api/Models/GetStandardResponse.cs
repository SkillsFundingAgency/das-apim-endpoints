using SFA.DAS.ManageApprenticeships.InnerApi.Responses;
using System;

namespace SFA.DAS.ManageApprenticeships.Api.Models
{
    public class GetStandardResponse
    {
        [Obsolete("Id is obsolete, use StandardUId or LarsCode")]
        public int Id { get; set; }
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public int MaxFunding { get; set; }

        public static implicit operator GetStandardResponse(GetStandardsListItem source)
        {
            return new GetStandardResponse
            {
                Id = source.LarsCode,
                StandardUId = source.StandardUId,
                LarsCode = source.LarsCode,
                Duration = source.TypicalDuration,
                Level = source.Level,
                Title = source.Title,
                MaxFunding = source.MaxFunding
            };
        }
    }
}