namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Notifications;

public class GetNotificationResponse
{
    public Guid MemberId { get; set; }
    public string TemplateName { get; set; } = string.Empty;
    public DateTime SentTime { get; set; }
    public string? ReferenceId { get; set; }
    public long? EmployerAccountId { get; set; }
}
