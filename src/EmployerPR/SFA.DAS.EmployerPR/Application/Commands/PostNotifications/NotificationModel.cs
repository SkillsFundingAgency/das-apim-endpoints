namespace SFA.DAS.EmployerPR.Application.Commands.PostNotifications;

public class NotificationModel
{
    public required string TemplateName { get; set; }
    public required string NotificationType { get; set; }
    public long? Ukprn { get; set; }
    public string? EmailAddress { get; set; }
    public string? Contact { get; set; }
    public string? EmployerName { get; set; }
    public Guid? RequestId { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public int? PermitApprovals { get; set; }
    public int? PermitRecruit { get; set; }
    public required string CreatedBy { get; set; }
}
