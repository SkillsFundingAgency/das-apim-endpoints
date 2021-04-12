using System;
using SFA.DAS.ApprenticeCommitments.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class GetRegistrationsNeedingRemindersRequest : IGetApiRequest
    {
        private readonly DateTime _invitationCutOffTime;

        public GetRegistrationsNeedingRemindersRequest(DateTime invitationCutOffTime)
        {
            _invitationCutOffTime = invitationCutOffTime;
        }
        public string GetUrl => $"registrations/reminders?invitationCutOffTime={_invitationCutOffTime.ToIsoDateTime()}";
    }
}