﻿using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships
{
    public class GetCurrentPartyIdsRequest : IGetApiRequest
    {
        public Guid ApprenticeshipKey { get; set; }
        public string GetUrl => $"{ApprenticeshipKey}/currentPartyIds";
    }
}
