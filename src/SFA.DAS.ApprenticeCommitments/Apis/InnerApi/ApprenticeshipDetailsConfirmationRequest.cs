using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class ApprenticeshipDetailsConfirmationRequest : CommitmentStatementConfirmationRequest<ApprenticeshipDetailsConfirmationRequestData>
    {
        public ApprenticeshipDetailsConfirmationRequest(
            Guid apprentice, long apprenticeship, long commitmentStatementId, bool apprenticeshipDetailsCorrect)
            : base(apprentice, apprenticeship, commitmentStatementId, "apprenticeshipdetailsconfirmation")
        {
            Data = new ApprenticeshipDetailsConfirmationRequestData
            {
                ApprenticeshipDetailsCorrect = apprenticeshipDetailsCorrect
            };
        }
    }

    public class ApprenticeshipDetailsConfirmationRequestData
    {
        public bool ApprenticeshipDetailsCorrect { get; set; }
    }
}