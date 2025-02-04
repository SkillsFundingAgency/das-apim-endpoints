using MediatR;

namespace SFA.DAS.EmployerAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

public record GetMemberNotificationSettingsQuery(Guid MemberId) : IRequest<GetMemberNotificationSettingsQueryResult>;