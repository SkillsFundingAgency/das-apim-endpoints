using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Enums;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Models;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses
{
    public record GetLegacyUserByEmailApiResponse
    {
        public string Username { get; init; } = null!;

        public UserStatuses Status { get; init; }

        public UserRoles Roles { get; init; }

        public string ActivationCode { get; init; } = null!;

        public DateTime? ActivateCodeExpiry { get; init; }

        public DateTime? ActivationDate { get; init; }

        public int LoginIncorrectAttempts { get; init; }

        public string PasswordResetCode { get; init; } = null!;

        public DateTime? PasswordResetCodeExpiry { get; init; }

        public int PasswordResetIncorrectAttempts { get; init; }

        public string AccountUnlockCode { get; init; } = null!;

        public DateTime? AccountUnlockCodeExpiry { get; init; }

        public DateTime? LastLogin { get; init; }

        public DateTime LastActivity { get; init; }

        public string PendingUsername { get; init; } = null!;

        public string PendingUsernameCode { get; init; } = null!;

        public int? LegacyCandidateId { get; set; }

        public Guid? SubscriberId { get; init; }

        public RegistrationDetails? RegistrationDetails { get; init; } = new();

        public ApplicationTemplate? ApplicationTemplate { get; init; } = new();

        public CommunicationPreferences? CommunicationPreferences { get; init; } = new();

        public HelpPreferences? HelpPreferences { get; init; } = new();

        public MonitoringInformation? MonitoringInformation { get; init; } = new();
    }
}
