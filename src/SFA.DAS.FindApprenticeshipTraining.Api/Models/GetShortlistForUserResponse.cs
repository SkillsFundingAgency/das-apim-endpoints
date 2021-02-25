using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetShortlistForUserResponse
    {
        public IEnumerable<GetShortlistItem> Shortlist { get; set; }
    }
}