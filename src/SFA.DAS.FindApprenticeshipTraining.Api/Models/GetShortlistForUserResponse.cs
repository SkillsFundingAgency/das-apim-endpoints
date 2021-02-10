using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetShortlistForUserResponse
    {
        private IEnumerable<GetShortlistItem> Shortlist { get; set; }
    }
}