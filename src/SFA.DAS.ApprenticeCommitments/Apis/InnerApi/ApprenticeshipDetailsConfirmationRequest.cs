using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{

    public class ApprenticeshipDetailsConfirmationRequest : IPostApiRequest<ApprenticeshipDetailsConfirmationRequestData>
    {
        private readonly Guid _apprenticeId;
        private readonly long _apprenticeshipId;

        public ApprenticeshipDetailsConfirmationRequest(
            Guid apprentice, long apprenticeship, bool employerCorrect)
        {
            _apprenticeId = apprentice;
            _apprenticeshipId = apprenticeship;
            Data = new ApprenticeshipDetailsConfirmationRequestData
            {
                ApprenticeshipDetailsCorrect = employerCorrect
            };
        }

        public string PostUrl => $"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/apprenticeshipdetailsconfirmation";

        public ApprenticeshipDetailsConfirmationRequestData Data { get; set; }
    }

    public class ApprenticeshipDetailsConfirmationRequestData
    {
        public bool ApprenticeshipDetailsCorrect { get; set; }
    }
}