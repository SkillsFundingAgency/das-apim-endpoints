using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{

    public class ApprenticeshipConfirmationRequest : IPostApiRequest<ApprenticeshipConfirmationRequestData>
    {
        private readonly Guid _apprenticeId;
        private readonly long _apprenticeshipId;

        public ApprenticeshipConfirmationRequest(
            Guid apprentice, long apprenticeship, bool apprenticeshipCorrect)
        {
            _apprenticeId = apprentice;
            _apprenticeshipId = apprenticeship;
            Data = new ApprenticeshipConfirmationRequestData
            {
                ApprenticeshipCorrect = apprenticeshipCorrect
            };
        }

        public string PostUrl => $"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/apprenticeshipconfirmation";

        public ApprenticeshipConfirmationRequestData Data { get; set; }
    }

    public class ApprenticeshipConfirmationRequestData
    {
        public bool ApprenticeshipCorrect { get; set; }
    }
}