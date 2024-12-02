namespace SFA.DAS.ProviderPR.InnerApi.Notifications.Commands;

public class NotificationModel
{
    public string TemplateName { get; set; } = null!;
    public string NotificationType { get; set; } = null!;
    public long? Ukprn { get; set; }
    public string? EmailAddress { get; set; }
    public string? Contact { get; set; }
    public string? EmployerName { get; set; }
    public Guid? RequestId { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public int? PermitApprovals { get; set; }
    public int? PermitRecruit { get; set; }
    public string CreatedBy { get; set; } = null!;

    public NotificationModel() { }

    public NotificationModel(string templateName, string notificationType, long? ukprn, string email, string contactName, long accountLegalEntityId, string createdBy, Guid? requestId)
    {
        TemplateName = templateName;
        NotificationType = notificationType;
        Ukprn = ukprn;
        EmailAddress = email;
        Contact = contactName;
        AccountLegalEntityId = accountLegalEntityId;
        CreatedBy = createdBy;
        RequestId = requestId;
    }
}
