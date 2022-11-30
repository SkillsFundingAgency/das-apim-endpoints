using System;

namespace SFA.DAS.EmployerAccounts.InnerApi.Responses
{
    public class GetReservationsResponseListItem
    {
        public Guid Id { get; set; }
        public long AccountId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public ReservationCourse Course { get; set; }
        public Status Status { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string AccountLegalEntityName { get; set; }
    }

    public class ReservationCourse
    {
        public string CourseId { get; set; }
        public string Title { get; set; }
        public string Level { get; set; }
    }

    public enum Status
    {
        Pending = 0,
        Confirmed = 1,
        Completed = 2,
        Deleted = 3
    }
}