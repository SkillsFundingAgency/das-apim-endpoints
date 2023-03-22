﻿using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticePortal.Apis.ApprenticeAccountsApi
{
    internal class GetApprenticeRequest : IGetApiRequest
    {
        public string GetUrl => $"apprentices/{ApprenticeId}";

        public Guid ApprenticeId { get; }

        public GetApprenticeRequest(Guid apprenticeId)
            => ApprenticeId = apprenticeId;
    }
}