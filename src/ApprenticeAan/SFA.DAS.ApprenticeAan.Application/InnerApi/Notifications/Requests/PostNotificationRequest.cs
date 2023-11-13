using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Infrastructure.Configuration;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Notifications;

public record PostNotificationRequest() : IRequest<GetNotificationResponse>
{
    [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)]
    public Guid RequestedByMemberId { get; set; }
    [FromQuery]
    public Guid MemberId { get; set; }
    [FromQuery]
    public int NotificationTemplateId { get; set; }
}
