using System;

namespace SFA.DAS.EpaoRegister.InnerApi.Responses
{
    public class GetStandardResponse
    {
        public int Id { get; set; }
        public int LarsCode { get; set; }
        public string StandardUId { get; set; }
        public GetStandardDates StandardDates { get; set; }
    }


    public class GetStandardDates
    {
        public DateTime? LastDateStarts { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime EffectiveFrom { get; set; }
    }
}
