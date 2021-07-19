using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class ChangeEmailAddressRequest : IPostApiRequest
    {
        private readonly object apprenticeId;

        public ChangeEmailAddressRequest(object apprenticeId)
            => this.apprenticeId = apprenticeId;

        public string PostUrl => $"/apprentices/{apprenticeId}/email";

        public object Data { get; set; }
    }

    public class ChangeEmailAddressRequestData
    {
        public string Email { get; set; }
    }

    public class ChangeEmailAddressResponse
    {
    }
}