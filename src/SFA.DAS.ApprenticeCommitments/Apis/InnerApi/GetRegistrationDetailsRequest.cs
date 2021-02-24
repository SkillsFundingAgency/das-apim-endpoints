using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class GetRegistrationDetailsRequest : IGetApiRequest
    {
        private readonly Guid _id;

        public GetRegistrationDetailsRequest(Guid id)
        {
            _id = id;
        }

        public string GetUrl => $"registrations/{_id}";
    }
}