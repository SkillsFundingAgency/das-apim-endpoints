using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

public record GetMemberNotificationSettingsQuery(Guid MemberId) : IRequest<GetMemberNotificationSettingsQueryResult>;
