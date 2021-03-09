using System;

namespace SFA.DAS.Reservations.InnerApi.Responses
{
    public class GetStandardsListItem
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public DateTime EffectiveTo { get; set; }
    }
}