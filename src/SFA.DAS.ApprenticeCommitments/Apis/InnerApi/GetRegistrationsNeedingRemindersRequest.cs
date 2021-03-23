using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class GetRegistrationsNeedingRemindersRequest : IGetApiRequest
    {
        public string GetUrl => "registrations/reminders";
    }
}