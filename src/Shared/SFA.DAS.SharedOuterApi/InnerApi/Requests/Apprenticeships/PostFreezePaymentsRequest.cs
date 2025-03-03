﻿using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;

public class PostFreezePaymentsRequest : IPostApiRequest
{
    public Guid ApprenticeshipKey { get; set; }
    public string PostUrl => $"{ApprenticeshipKey}/freeze";
    public object Data { get; set; }

    public PostFreezePaymentsRequest(Guid apprenticeshipKey, string reason)
    {
        ApprenticeshipKey = apprenticeshipKey;
        Data = new FreezePaymentsRequestData { Reason = reason };
    }
}

public class FreezePaymentsRequestData
{
    public string Reason { get; set; }
}