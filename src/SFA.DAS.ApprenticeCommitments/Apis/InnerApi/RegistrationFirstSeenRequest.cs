using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class RegistrationFirstSeenRequest : IPostApiRequest<RegistrationFirstSeenRequestData>
    {
        private readonly Guid _apprenticeId;

        public RegistrationFirstSeenRequest(Guid apprentice, RegistrationFirstSeenRequestData data)
        {
            _apprenticeId = apprentice;
            Data =  data;
        }

        public string PostUrl => $"/registrations/{_apprenticeId}/firstseen";

        public RegistrationFirstSeenRequestData Data { get; set; }
    }

    public class RegistrationFirstSeenRequestData
    {
        public DateTime SeenOn { get; set; }
    }
}