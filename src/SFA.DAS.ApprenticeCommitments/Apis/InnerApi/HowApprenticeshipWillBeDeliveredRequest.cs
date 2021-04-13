using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{

    public class HowApprenticeshipWillBeDeliveredRequest : IPostApiRequest<HowApprenticeshipWillBeDeliveredRequestData>
    {
        private readonly Guid _apprenticeId;
        private readonly long _apprenticeshipId;

        public HowApprenticeshipWillBeDeliveredRequest(
            Guid apprentice, long apprenticeship, bool howApprenticeshipDeliveredCorrect)
        {
            _apprenticeId = apprentice;
            _apprenticeshipId = apprenticeship;
            Data = new HowApprenticeshipWillBeDeliveredRequestData
            {
                HowApprenticeshipDeliveredCorrect = howApprenticeshipDeliveredCorrect
            };
        }

        public string PostUrl => $"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/howapprenticeshipwillbedeliveredconfirmation";

        public HowApprenticeshipWillBeDeliveredRequestData Data { get; set; }
    }

    public class HowApprenticeshipWillBeDeliveredRequestData
    {
        public bool HowApprenticeshipDeliveredCorrect { get; set; }
    }
}