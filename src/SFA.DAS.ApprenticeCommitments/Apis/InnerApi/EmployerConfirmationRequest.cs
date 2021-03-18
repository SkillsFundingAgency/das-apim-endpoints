using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class EmployerConfirmationRequest : IPostApiRequest<EmployerConfirmationRequestData>
    {
        private readonly Guid _apprenticeId;
        private readonly long _apprenticeshipId;

        public EmployerConfirmationRequest(
            Guid apprentice, long apprenticeship, bool trainingProviderCorrect)
        {
            _apprenticeId = apprentice;
            _apprenticeshipId = apprenticeship;
            Data = new EmployerConfirmationRequestData
            {
                EmployerCorrect = trainingProviderCorrect
            };
        }

        public string PostUrl => $"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/employerconfirmation";

        public EmployerConfirmationRequestData Data { get; set; }
    }

    public class EmployerConfirmationRequestData
    {
        public bool EmployerCorrect { get; set; }
    }
}