using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeCommitments.Requests
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
