using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class ApplicationDetailsResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
        public OpportunityData Opportunity { get; set; }
        public IEnumerable<ReferenceDataItem> Sectors { get; set; }
        public IEnumerable<ReferenceDataItem> JobRoles { get; set; }

        public IEnumerable<ReferenceDataItem> Levels { get; set; }

        public class OpportunityData
        {
            public int Id { get; set; }
            public long AccountId { get; set; }

            public int Amount { get; set; }
            public int RemainingAmount { get; set; }
            public bool IsNamePublic { get; set; }

            public string DasAccountName { get; set; }

            public IEnumerable<string> JobRoles { get; set; }

            public IEnumerable<string> Levels { get; set; }

            public IEnumerable<string> Sectors { get; set; }

            public IEnumerable<string> Locations { get; set; }

            public static implicit operator OpportunityData(Pledge pledge)
            {
                return new OpportunityData()
                {
                    Id = pledge.Id.Value,
                    Amount = pledge.Amount,
                    RemainingAmount = pledge.RemainingAmount,
                    DasAccountName = pledge.IsNamePublic ? pledge.DasAccountName : "Opportunity",
                    IsNamePublic = pledge.IsNamePublic,
                    JobRoles = pledge.JobRoles,
                    Levels = pledge.Levels,
                    Sectors = pledge.Sectors,
                    Locations = pledge.Locations.Select(x => x.Name)
                };
            }
        }
    }
}