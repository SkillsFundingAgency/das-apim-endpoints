using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class EmployerConfirmationRequest : CommitmentStatementConfirmationRequest<EmployerConfirmationRequestData>
    {
        public EmployerConfirmationRequest(
            Guid apprentice, long apprenticeship, long commitmentStatementId, bool employerCorrect)
            : base(apprentice, apprenticeship, commitmentStatementId, "employerconfirmation")
        {
            Data = new EmployerConfirmationRequestData
            {
                EmployerCorrect = employerCorrect
            };
        }
    }

    public class EmployerConfirmationRequestData
    {
        public bool EmployerCorrect { get; set; }
    }
}