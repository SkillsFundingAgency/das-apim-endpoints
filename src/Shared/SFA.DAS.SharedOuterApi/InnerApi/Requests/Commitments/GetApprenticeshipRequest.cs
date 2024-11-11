﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments
{
    public class GetApprenticeshipRequest : IGetApiRequest
    {
        public readonly long ApprenticeshipId;

        public GetApprenticeshipRequest(long apprenticeshipId)
        {
            ApprenticeshipId = apprenticeshipId;
        }

        public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}";
    }
}
