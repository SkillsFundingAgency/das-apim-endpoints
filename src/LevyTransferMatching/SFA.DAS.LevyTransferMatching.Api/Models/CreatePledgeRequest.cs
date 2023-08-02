using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class CreatePledgeRequest
    {
        [Required]
        public int Amount { get; set; }

        [Required]
        public bool IsNamePublic { get; set; }

        [Required]
        public string DasAccountName { get; set; }

        [Required]
        public IEnumerable<string> Sectors { get; set; }

        [Required]
        public IEnumerable<string> JobRoles { get; set; }

        [Required]
        public IEnumerable<string> Levels { get; set; }

        [Required]
        public List<string> Locations { get; set; }
        public string AutomaticApprovalOption { get; set; }

        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
    }
}