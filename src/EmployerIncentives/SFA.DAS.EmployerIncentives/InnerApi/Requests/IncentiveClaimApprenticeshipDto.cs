using System;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class IncentiveClaimApprenticeshipDto
    {
        public long ApprenticeshipId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public long Uln { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public ApprenticeshipEmployerType ApprenticeshipEmployerTypeOnApproval { get; set; }
        public long UKPRN { get; set; }
        public string CourseName { get; set; }
        public DateTime? EmploymentStartDate { get; set; }

        public static implicit operator IncentiveClaimApprenticeshipDto(ApprenticeshipResponse from)
        {
            return new IncentiveClaimApprenticeshipDto
            {
                ApprenticeshipId = from.Id,
                FirstName = from.FirstName,
                LastName = from.LastName,
                DateOfBirth = from.DateOfBirth,
                Uln = from.Uln,
                PlannedStartDate = from.StartDate,
                ApprenticeshipEmployerTypeOnApproval = MapLevyType(from.ApprenticeshipEmployerTypeOnApproval),
                UKPRN = from.ProviderId,
                CourseName = from.CourseName
            };
        }

        private static ApprenticeshipEmployerType MapLevyType(InnerApi.Responses.Commitments.ApprenticeshipEmployerType? from)
        {

            switch (from)
            {
                case null:
                    return ApprenticeshipEmployerType.Unknown;
                case InnerApi.Responses.Commitments.ApprenticeshipEmployerType.Levy:
                    return ApprenticeshipEmployerType.Levy;
                case InnerApi.Responses.Commitments.ApprenticeshipEmployerType.NonLevy:
                    return ApprenticeshipEmployerType.NonLevy;
                default:
                    throw new InvalidCastException($"Unable to convert from Commitments.ApprenticeshipEmployerType {from}");
            }
        }
    }
}
