using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{

    public class GetApprenticeshipsRequest : IGetApiRequest
    {
        private readonly Guid _apprenticeId;

        public GetApprenticeshipsRequest(Guid apprenticeId)
        => _apprenticeId = apprenticeId;

        public string GetUrl => $"apprentices/{_apprenticeId}/apprenticeships";
    }

    public class ApprenticeshipsRepsonse
    {
        public long Id { get; set; }
    }
}