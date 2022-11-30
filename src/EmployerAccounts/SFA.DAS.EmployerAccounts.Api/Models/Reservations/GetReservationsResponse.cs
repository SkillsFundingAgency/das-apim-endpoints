using System;
using System.Collections.Generic;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;

namespace SFA.DAS.EmployerAccounts.Api.Models.Reservations
{
    public class GetReservationsResponse
    {
        public IEnumerable<ReservationsResponse> Reservations { get; set; }
    }

    public class ReservationsResponse
    {
        public Guid Id { get; set; }
        public long AccountId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Course Course { get; set; }
        public ReservationStatus Status { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string AccountLegalEntityName { get; set; }

        public static implicit operator ReservationsResponse(GetReservationsResponseListItem source)
        {
            return new ReservationsResponse
            {
                Id = source.Id,
                AccountId = source.AccountId,
                CreatedDate = source.CreatedDate,
                StartDate = source.StartDate,
                ExpiryDate = source.ExpiryDate,
                Course = source.Course,
                Status = (ReservationStatus)source.Status,
                AccountLegalEntityId = source.AccountLegalEntityId,
                AccountLegalEntityName = source.AccountLegalEntityName
            };
        }
    }

    public class Course
    {
        public string CourseId { get; set; }
        public string Title { get; set; }
        public string Level { get; set; }

        public static implicit operator Course(ReservationCourse source)
        {
            return new Course
            {
                CourseId = source.CourseId,
                Title = source.Title,
                Level = source.Level
            };
        }
    }

    public enum ReservationStatus
    {
        Pending = 0,
        Confirmed = 1,
        Completed = 2,
        Deleted = 3
    }
}