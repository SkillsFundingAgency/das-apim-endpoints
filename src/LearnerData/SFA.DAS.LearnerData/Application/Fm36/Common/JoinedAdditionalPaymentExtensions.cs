namespace SFA.DAS.LearnerData.Application.Fm36.Common;

public static class JoinedAdditionalPaymentExtensions
{
    public static bool IsIncentive(this JoinedAdditionalPayment additionalPayment)
    {
        return EarningsFM36Constants.AdditionalPaymentsTypes.Incentives.Contains(additionalPayment
            .AdditionalPaymentType);
    }
}
