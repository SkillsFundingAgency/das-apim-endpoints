namespace SFA.DAS.EmployerAan.Application.MemberNotificationEventFormat.Queries.GetMemberNotificationEventFormats
{
    public class MemberNotificationEventFormatModel
    {
        public long Id { get; set; }
        public Guid MemberId { get; set; }
        public string EventFormat { get; set; } = string.Empty;
        public int Ordering { get; set; }
        public bool ReceiveNotifications { get; set; }
    }
}