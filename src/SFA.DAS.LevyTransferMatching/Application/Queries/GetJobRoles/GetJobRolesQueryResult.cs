﻿using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.Tags;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetJobRoles
{
    public class GetJobRolesQueryResult
    {
        public IEnumerable<Tag> Tags { get; set; }
    }
}
