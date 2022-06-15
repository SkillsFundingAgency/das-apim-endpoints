using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public abstract class CommitmentStatementConfirmationRequest<T> : IPostApiRequest<T>
    {
        private readonly Guid _apprenticeId;
        private readonly long _apprenticeshipId;
        private readonly long _commitmentStatementId;
        public readonly string _confirmationPath;

        public CommitmentStatementConfirmationRequest(
            Guid apprentice, long apprenticeship, long commitmentStatementId, string confirmationPath)
        {
            _apprenticeId = apprentice;
            _apprenticeshipId = apprenticeship;
            _commitmentStatementId = commitmentStatementId;
            _confirmationPath = confirmationPath;
        }

        public string PostUrl => $"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/revisions/{_commitmentStatementId}/{_confirmationPath}";

        public T Data { get; set; }
    }
}