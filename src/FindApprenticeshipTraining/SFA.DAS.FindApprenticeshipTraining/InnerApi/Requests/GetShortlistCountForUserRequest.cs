﻿using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public record GetShortlistCountForUserRequest(Guid UserId) : IGetApiRequest
{
    public string GetUrl => $"api/shortlists/users/{UserId}/count";
}
