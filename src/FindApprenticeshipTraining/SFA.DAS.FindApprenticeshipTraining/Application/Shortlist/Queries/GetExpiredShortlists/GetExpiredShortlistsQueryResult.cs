using System;
using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetExpiredShortlists
{
    public class GetExpiredShortlistsQueryResult
    {
        public IEnumerable<Guid> UserIds { get; set; }
    }
}