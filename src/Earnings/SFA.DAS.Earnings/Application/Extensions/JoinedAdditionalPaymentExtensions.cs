using SFA.DAS.Earnings.Application.Earnings;

namespace SFA.DAS.Earnings.Application.Extensions
{
    public static class JoinedAdditionalPaymentExtensions
    {
        public static bool IsIncentive(this JoinedAdditionalPayment additionalPayment)
        {
            return EarningsFM36Constants.AdditionalPaymentsTypes.Incentives.Contains(additionalPayment
                .AdditionalPaymentType);
        }
    }
}
