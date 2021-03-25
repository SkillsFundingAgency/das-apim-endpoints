using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class TrainingProviderConfirmationRequest : IPostApiRequest<TrainingProviderConfirmationRequestData>
    {
        private readonly Guid _apprenticeId;
        private readonly long _apprenticeshipId;

        public TrainingProviderConfirmationRequest(
            Guid apprentice, long apprenticeship, bool trainingProviderCorrect)
        {
            _apprenticeId = apprentice;
            _apprenticeshipId = apprenticeship;
            Data = new TrainingProviderConfirmationRequestData
            {
                TrainingProviderCorrect = trainingProviderCorrect
            };
        }

        public string PostUrl => $"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/trainingproviderconfirmation";

        public TrainingProviderConfirmationRequestData Data { get; set; }
    }

    public class TrainingProviderConfirmationRequestData
    {
        public bool TrainingProviderCorrect { get; set; }
    }
}