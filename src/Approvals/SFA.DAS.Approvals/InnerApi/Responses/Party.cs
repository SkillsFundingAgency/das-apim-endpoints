using System;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    [Flags]
    public enum Party : short
    {
        None = 0,
        Employer = 1,
        Provider = 2,
        TransferSender = 4
    }
}
