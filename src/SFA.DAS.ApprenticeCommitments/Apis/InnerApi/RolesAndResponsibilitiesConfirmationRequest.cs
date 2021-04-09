using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class RolesAndResponsibilitiesConfirmationRequest : IPostApiRequest<RolesAndResponsibilitiesConfirmationRequestData>
    {
        private readonly Guid _apprenticeId;
        private readonly long _apprenticeshipId;

        public RolesAndResponsibilitiesConfirmationRequest(
            Guid apprentice, long apprenticeship, bool rolesAndResponsibilitiesCorrect)
        {
            _apprenticeId = apprentice;
            _apprenticeshipId = apprenticeship;
            Data = new RolesAndResponsibilitiesConfirmationRequestData
            {
                RolesAndResponsibilitiesCorrect = rolesAndResponsibilitiesCorrect
            };
        }

        public string PostUrl => $"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/rolesandresponsibilitiesconfirmation";

        public RolesAndResponsibilitiesConfirmationRequestData Data { get; set; }
    }

    public class RolesAndResponsibilitiesConfirmationRequestData
    {
        public bool RolesAndResponsibilitiesCorrect { get; set; }
    }
}