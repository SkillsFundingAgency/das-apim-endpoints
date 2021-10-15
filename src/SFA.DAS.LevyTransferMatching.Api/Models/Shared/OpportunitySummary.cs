using SFA.DAS.LevyTransferMatching.Models;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Shared
{
    public class OpportunitySummary
    {
        public int Id { get; set; }
        public string DasAccountName { get; set; }
        public int Amount { get; set; }
        public bool IsNamePublic { get; set; }
        public IEnumerable<string> JobRoles { get; set; }
        public IEnumerable<string> Levels { get; set; }
        public IEnumerable<string> Sectors { get; set; }
        public IEnumerable<string> Locations { get; set; }

        public static implicit operator OpportunitySummary(Pledge pledge)
        {
            return new OpportunitySummary()
            {
                Id = pledge.Id.Value,
                Amount = pledge.RemainingAmount,
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