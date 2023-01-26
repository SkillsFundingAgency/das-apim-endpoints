using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.InnerApi.Interfaces;

namespace SFA.DAS.Approvals.Extensions
{
    public static class IPartyResourceExtensions
    {
        public static bool CheckParty(this IPartyResource resource, ServiceParameters serviceParameters)
        {
            switch (serviceParameters.CallingParty)
            {
                case Application.Shared.Enums.Party.Employer:
                {
                    if (resource.AccountId != serviceParameters.CallingPartyId)
                    {
                        return false;
                    }

                    break;
                }
                case Application.Shared.Enums.Party.Provider:
                {
                    if (resource.ProviderId != serviceParameters.CallingPartyId)
                    {
                        return false;
                    }

                    break;
                }
                default:
                    return false;
            }

            return true;
        }
    }
}
