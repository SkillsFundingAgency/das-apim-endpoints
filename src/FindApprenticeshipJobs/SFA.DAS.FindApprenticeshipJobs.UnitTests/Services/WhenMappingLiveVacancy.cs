using System.Net;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Services
{
    [TestFixture]
    public class WhenMappingLiveVacancy
    {
        [Test, MoqAutoData]
        public void Then_The_Vacancy_Is_Mapped(
            LiveVacancy source,
            [Frozen] Mock<ICourseService> courseService,
            LiveVacancyMapper sut)
        {
            var mockStandardsListResponse =  SetupCoursesApiResponse(source);

            var result = sut.Map(source, mockStandardsListResponse);

            AssertResponse(result, source, mockStandardsListResponse);
        }

        private static void AssertResponse(FindApprenticeshipJobs.Application.Shared.LiveVacancy actual, LiveVacancy source, GetStandardsListResponse standardsListResponse)
        {
            var expectedResult = new
            {
                Id = source.VacancyReference.ToString(),
                source.VacancyReference,
                source.VacancyId,
                source.Title,
                PostedDate = source.LiveDate,
                source.StartDate,
                source.ClosingDate,
                Description = source.ShortDescription,
                source.NumberOfPositions,
                source.EmployerName,
                ProviderName = source.TrainingProvider.Name,
                source.TrainingProvider.Ukprn,
                IsPositiveAboutDisability = false,
                IsEmployerAnonymous = source.IsAnonymous,
                VacancyLocationType = "NonNational",
                ApprenticeshipLevel = "Higher",
                Wage = source.Wage == null ? null : new FindApprenticeshipJobs.Application.Shared.Wage
                {
                    Duration = source.Wage.Duration,
                    DurationUnit = source.Wage.DurationUnit,
                    FixedWageYearlyAmount = source.Wage.FixedWageYearlyAmount,
                    WageAdditionalInformation = source.Wage.WageAdditionalInformation,
                    WageType = source.Wage.WageType,
                    WeeklyHours = source.Wage.WeeklyHours,
                    WorkingWeekDescription = source.Wage.WorkingWeekDescription,
                    ApprenticeMinimumWage = source.Wage.ApprenticeMinimumWage,
                    Under18NationalMinimumWage = source.Wage.Under18NationalMinimumWage,
                    Between18AndUnder21NationalMinimumWage = source.Wage.Between18AndUnder21NationalMinimumWage,
                    Between21AndUnder25NationalMinimumWage = source.Wage.Between21AndUnder25NationalMinimumWage,
                    Over25NationalMinimumWage = source.Wage.Over25NationalMinimumWage
                },
                AnonymousEmployerName = source.IsAnonymous ? source.EmployerName: null,
                IsDisabilityConfident = source.DisabilityConfident == DisabilityConfident.Yes,
                source.AccountPublicHashedId,
                source.AccountLegalEntityPublicHashedId,
                LongDescription = source.Description,
                source.TrainingDescription,
                source.Skills,
                Qualifications = source.Qualifications.Select(q => new Qualification
                {
                    QualificationType = q.QualificationType,
                    Subject = q.Subject,
                    Grade = q.Grade,
                    Weighting = q.Weighting
                }),
                source.OutcomeDescription,
                ApprenticeshipTitle = standardsListResponse.Standards.Single(s => s.LarsCode.ToString() == source.ProgrammeId).Title,
                standardsListResponse.Standards.Single(s => s.LarsCode.ToString() == source.ProgrammeId).Level,
                StandardLarsCode = standardsListResponse.Standards.Single(s => s.LarsCode.ToString() == source.ProgrammeId).LarsCode,
                
                standardsListResponse.Standards.Single(s => s.LarsCode.ToString() == source.ProgrammeId).Route,
                standardsListResponse.Standards.Single(s => s.LarsCode.ToString() == source.ProgrammeId).RouteCode,
                Address = new FindApprenticeshipJobs.Application.Shared.Address
                {
                    AddressLine1 = source.EmployerLocation?.AddressLine1,
                    AddressLine2 = source.EmployerLocation?.AddressLine2,
                    AddressLine3 = source.EmployerLocation?.AddressLine3,
                    AddressLine4 = source.EmployerLocation?.AddressLine4,
                    Postcode = source.EmployerLocation?.Postcode,
                    Latitude = source.EmployerLocation?.Latitude ?? 0,
                    Longitude = source.EmployerLocation?.Longitude ?? 0,
                },
                
            };

            actual.Should().BeEquivalentTo(expectedResult);
        }


        private static GetStandardsListResponse SetupCoursesApiResponse(LiveVacancy vacancy)
        {
            var fixture = new Fixture();

            var larsCode = fixture.Create<int>();
            vacancy.ProgrammeId = larsCode.ToString();

            var standards = new List<GetStandardsListResponse.GetStandardsListItem>
            {
                new GetStandardsListResponse.GetStandardsListItem
                {
                    LarsCode = larsCode,
                    Level = 4,
                    Title = fixture.Create<string>(),
                    Route = fixture.Create<string>()
                }
            };

            var result = new ApiResponse<GetStandardsListResponse>(new GetStandardsListResponse
            { Standards = standards, Total = standards.Count, TotalFiltered = standards.Count }, HttpStatusCode.OK, string.Empty);
            
            return result.Body;
        }
    }
}
