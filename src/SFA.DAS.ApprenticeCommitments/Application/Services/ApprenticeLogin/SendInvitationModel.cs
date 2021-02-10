using System;

namespace SFA.DAS.ApprenticeCommitments.Application.Services.ApprenticeLogin
{
    public class SendInvitationModel
    {
        public Guid SourceId { get; set; }
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string OrganisationName { get; set; }
        public string ApprenticeshipName { get; set; }
    }
}