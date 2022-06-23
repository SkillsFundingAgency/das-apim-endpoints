using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class TrainingProviderConfirmationRequest : CommitmentStatementConfirmationRequest<TrainingProviderConfirmationRequestData>
    {
        public TrainingProviderConfirmationRequest(
            Guid apprentice, long apprenticeship, long commitmentStatementId, bool trainingProviderCorrect)
            : base(apprentice, apprenticeship, commitmentStatementId, "trainingproviderconfirmation")
        {
            Data = new TrainingProviderConfirmationRequestData
            {
                TrainingProviderCorrect = trainingProviderCorrect
            };
        }
    }

    public class TrainingProviderConfirmationRequestData
    {
        public bool TrainingProviderCorrect { get; set; }
    }
}