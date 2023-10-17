﻿using System.Collections.Generic;
using Microsoft.Azure.Amqp.Serialization;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterests
{
    public class BrowseByInterestsResult
    {
        public List<GetRoutesListItem> Routes { get; set; }
    }
}