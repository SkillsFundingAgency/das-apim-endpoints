using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.RecruitJobs.Api.Models.Mappers;

public static class DelayedNotificationExtensions
{
    private static NotificationEmail ToResponseDto(InnerApi.Responses.DelayedNotifications.NotificationEmail source)
    {
        return new NotificationEmail
        {
            RecipientAddress = source.RecipientAddress,
            SourceIds = source.SourceIds,
            TemplateId = source.TemplateId,
            Tokens = source.Tokens,
        };
    }
    
    public static List<NotificationEmail> ToGetResponse(this List<InnerApi.Responses.DelayedNotifications.NotificationEmail> notifications)
    {
        return notifications?.Select(ToResponseDto).ToList() ?? [];
    }
}