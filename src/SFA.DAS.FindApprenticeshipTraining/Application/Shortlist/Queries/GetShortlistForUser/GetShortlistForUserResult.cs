using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistForUser
{
    public class GetShortlistForUserResult
    {
        public IEnumerable<GetShortlistItem> Shortlist { get; set; }
    }
}