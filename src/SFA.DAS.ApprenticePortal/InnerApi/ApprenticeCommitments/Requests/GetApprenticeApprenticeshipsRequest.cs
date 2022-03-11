using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticePortal.InnerApi.ApprenticeCommitments.Requests
{
    public class GetApprenticeApprenticeshipsRequest : IGetApiRequest
    {
        private readonly Guid _id;

        public GetApprenticeApprenticeshipsRequest(Guid id)
        {
            _id = id;
        }

        public string GetUrl => $"apprentices/{_id}/apprenticeships";
    }
}
