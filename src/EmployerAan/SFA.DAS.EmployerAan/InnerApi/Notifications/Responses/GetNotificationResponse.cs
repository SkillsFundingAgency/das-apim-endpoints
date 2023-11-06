namespace SFA.DAS.EmployerAan.InnerApi.Notifications.Responses;

public class GetNotificationResponse
{
    public Guid MemberId { get; set; }
    public string TemplateName { get; set; } = null!;
    public DateTime SentTime { get; set; }
    public string? ReferenceId { get; set; }
    public long? EmployerAccountId { get; set; }
}
