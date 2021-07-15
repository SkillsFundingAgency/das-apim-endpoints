using System;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class ApprenticeshipIncentiveDto
    {
        public Guid Id { get; set; }
        public long ApprenticeshipId { get; set; }
        public long ULN { get; set; }
        public long? UKPRN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime StartDate { get; set; }
        public string CourseName { get; set; }

        public static implicit operator ApprenticeshipIncentiveDto(InnerApi.Responses.ApprenticeshipIncentiveDto source)
        {
            return new ApprenticeshipIncentiveDto
            {
                ApprenticeshipId = source.ApprenticeshipId,
                CourseName = source.CourseName,
                FirstName = source.FirstName,
                Id = source.Id,
                LastName = source.LastName,
                StartDate = source.StartDate,
                UKPRN = source.UKPRN,
                ULN = source.ULN
            };
        }
    }
}