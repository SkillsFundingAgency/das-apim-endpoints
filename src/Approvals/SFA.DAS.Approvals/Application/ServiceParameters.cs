using SFA.DAS.Approvals.Application.Shared.Enums;

namespace SFA.DAS.Approvals.Application
{
    public class ServiceParameters
    {
        public Party CallingParty { get; }
        public long CallingPartyId { get; }

        public ServiceParameters(Party callingParty, long callingPartyId)
        {
            CallingParty = callingParty;
            CallingPartyId = callingPartyId;
        }
    }
}
