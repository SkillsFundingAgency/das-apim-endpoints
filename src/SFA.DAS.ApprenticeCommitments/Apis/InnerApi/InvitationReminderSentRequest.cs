using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class InvitationReminderSentRequest : IPostApiRequest<InvitationReminderSentData>
    {
        private readonly Guid _apprenticeId;

        public InvitationReminderSentRequest(Guid apprenticeId)
        {
            _apprenticeId = apprenticeId;
        }

        public string PostUrl => $"/registrations/{_apprenticeId}/reminder";

        public InvitationReminderSentData Data { get; set; }
    }

    public class InvitationReminderSentData
    {
        public DateTime SentOn { get; set; }
    }
}