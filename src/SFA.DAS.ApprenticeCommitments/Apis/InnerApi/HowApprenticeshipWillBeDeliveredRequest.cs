using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class HowApprenticeshipWillBeDeliveredRequest : CommitmentStatementConfirmationRequest<HowApprenticeshipWillBeDeliveredRequestData>
    {
        public HowApprenticeshipWillBeDeliveredRequest(
            Guid apprentice, long apprenticeship, long commitmentStatementId, bool howApprenticeshipDeliveredCorrect)
            : base(apprentice, apprenticeship, commitmentStatementId, "howapprenticeshipwillbedeliveredconfirmation")
        {
            Data = new HowApprenticeshipWillBeDeliveredRequestData
            {
                HowApprenticeshipDeliveredCorrect = howApprenticeshipDeliveredCorrect
            };
        }
    }

    public class HowApprenticeshipWillBeDeliveredRequestData
    {
        public bool HowApprenticeshipDeliveredCorrect { get; set; }
    }
}