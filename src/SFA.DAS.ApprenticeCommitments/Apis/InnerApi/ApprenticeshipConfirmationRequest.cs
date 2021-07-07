using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class ApprenticeshipConfirmationRequest : CommitmentStatementConfirmationRequest<ApprenticeshipConfirmationRequestData>
    {
        public ApprenticeshipConfirmationRequest(
            Guid apprentice, long apprenticeship, long commitmentStatementId, bool apprenticeshipCorrect)
            : base(apprentice, apprenticeship, commitmentStatementId, "apprenticeshipconfirmation")
        {
            Data = new ApprenticeshipConfirmationRequestData
            {
                ApprenticeshipCorrect = apprenticeshipCorrect
            };
        }
    }

    public class ApprenticeshipConfirmationRequestData
    {
        public bool ApprenticeshipCorrect { get; set; }
    }
}