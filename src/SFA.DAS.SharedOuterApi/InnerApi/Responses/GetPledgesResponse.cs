using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetPledgesResponse
    {
        public IEnumerable<Pledge> Pledges { get; set; }
        public int TotalPledges { get; set; }

        public class Pledge
        {
            public int Id { get; set; }
            public long AccountId { get; set; }
            public int Amount { get; set; }
            public int RemainingAmount { get; set; }
            public int ApplicationCount { get; set; }
            public bool IsNamePublic { get; set; }
            public string DasAccountName { get; set; }
            public DateTime CreatedOn { get; set; }
            public IEnumerable<string> Sectors { get; set; }
            public IEnumerable<string> JobRoles { get; set; }
            public IEnumerable<string> Levels { get; set; }
            public List<LocationDataItem> Locations { get; set; }

            public class LocationDataItem
            {
                public string Name { get; set; }
                public double[] GeoPoint { get; set; }
            }
        }
    }
}