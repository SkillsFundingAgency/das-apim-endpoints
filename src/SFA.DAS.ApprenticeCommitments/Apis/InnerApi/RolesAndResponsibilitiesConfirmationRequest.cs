using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class RolesAndResponsibilitiesConfirmationRequest : CommitmentStatementConfirmationRequest<RolesAndResponsibilitiesConfirmationRequestData>
    {
        public RolesAndResponsibilitiesConfirmationRequest(
            Guid apprentice, long apprenticeship, long commitmentStatementId, bool rolesAndResponsibilitiesCorrect)
            : base(apprentice, apprenticeship, commitmentStatementId, "rolesandresponsibilitiesconfirmation")
        {
            Data = new RolesAndResponsibilitiesConfirmationRequestData
            {
                RolesAndResponsibilitiesCorrect = rolesAndResponsibilitiesCorrect
            };
        }
    }

    public class RolesAndResponsibilitiesConfirmationRequestData
    {
        public bool RolesAndResponsibilitiesCorrect { get; set; }
    }
}