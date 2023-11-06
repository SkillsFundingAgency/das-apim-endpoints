using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.Notifications.Responses;

namespace SFA.DAS.EmployerAan.Application.InnerApi.Notifications;

public record PostNotificationRequest() : IRequest<GetNotificationResponse>
{
    [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)]
    public Guid RequestedByMemberId { get; set; }
    [FromQuery]
    public Guid MemberId { get; set; }
    [FromQuery]
    public int NotificationTemplateId { get; set; }
}
