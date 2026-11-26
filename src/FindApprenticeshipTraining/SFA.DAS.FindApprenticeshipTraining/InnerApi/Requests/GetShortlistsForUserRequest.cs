using System;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public record GetShortlistsForUserRequest(Guid UserId) : IGetApiRequest
{
    public string GetUrl => $"shortlists/users/{UserId}";
}
