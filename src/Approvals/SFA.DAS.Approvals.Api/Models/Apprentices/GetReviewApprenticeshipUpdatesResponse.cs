using SFA.DAS.Approvals.InnerApi;
using System;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetReviewApprenticeshipUpdates;

namespace SFA.DAS.Approvals.Api.Models.Apprentices
{
    public class GetReviewApprenticeshipUpdatesResponse
    {
        public bool IsValidCourseCode { get; set; }
        public string ProviderName { get; set; }
        public string EmployerName { get; set; }
        public ApprenticeshipDetails OriginalApprenticeship { get; set; }
        public ApprenticeshipDetails ApprenticeshipUpdates { get; set; }

        public static implicit operator GetReviewApprenticeshipUpdatesResponse(GetReviewApprenticeshipUpdatesQueryResult source)
        {
            return new GetReviewApprenticeshipUpdatesResponse
            {
                IsValidCourseCode = source.IsValidCourseCode,
                ProviderName = source.ProviderName,
                EmployerName = source.EmployerName,
                OriginalApprenticeship = new ApprenticeshipDetails
                {
                    FirstName = source.OriginalApprenticeship.FirstName,
                    LastName = source.OriginalApprenticeship.LastName,
                    Email = source.OriginalApprenticeship.Email,
                    Uln = source.OriginalApprenticeship.Uln,
                    DateOfBirth = source.OriginalApprenticeship.DateOfBirth,
                    StartDate = source.OriginalApprenticeship.StartDate,
                    EndDate = source.OriginalApprenticeship.EndDate,
                    CourseCode = source.OriginalApprenticeship.CourseCode,
                    CourseName = source.OriginalApprenticeship.CourseName,
                    Version = source.OriginalApprenticeship.Version,
                    Option = source.OriginalApprenticeship.Option,
                    DeliveryModel = source.OriginalApprenticeship.DeliveryModel,
                    EmploymentEndDate = source.OriginalApprenticeship.EmploymentEndDate,
                    EmploymentPrice = source.OriginalApprenticeship.EmploymentPrice,
                    Cost = source.OriginalApprenticeship.Cost,
                },
                ApprenticeshipUpdates = new ApprenticeshipDetails
                {
                    FirstName = source.ApprenticeshipUpdates.FirstName,
                    LastName = source.ApprenticeshipUpdates.LastName,
                    Email = source.ApprenticeshipUpdates.Email,
                    Uln = source.ApprenticeshipUpdates.Uln,
                    DateOfBirth = source.ApprenticeshipUpdates.DateOfBirth,
                    StartDate = source.ApprenticeshipUpdates.StartDate,
                    EndDate = source.ApprenticeshipUpdates.EndDate,
                    CourseCode = source.ApprenticeshipUpdates.CourseCode,
                    CourseName = source.ApprenticeshipUpdates.CourseName,
                    Version = source.ApprenticeshipUpdates.Version,
                    Option = source.ApprenticeshipUpdates.Option,
                    DeliveryModel = source.ApprenticeshipUpdates.DeliveryModel,
                    EmploymentEndDate = source.ApprenticeshipUpdates.EmploymentEndDate,
                    EmploymentPrice = source.ApprenticeshipUpdates.EmploymentPrice,
                    Cost = source.ApprenticeshipUpdates.Cost,
                },
            };
        }
    }

    public class ApprenticeshipDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Uln { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Version { get; set; }
        public string Option { get; set; }
        public DeliveryModel? DeliveryModel { get; set; }
        public DateTime? EmploymentEndDate { get; set; }
        public int? EmploymentPrice { get; set; }
        public Decimal? Cost { get; set; }
    }
}
