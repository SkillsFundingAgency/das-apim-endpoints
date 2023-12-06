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
                source.VacancyId,
                VacancyTitle = source.Title,
                source.VacancyReference,
                ApprenticeshipTitle = standardsListResponse.Standards.Single(s => s.LarsCode.ToString() == source.ProgrammeId).Title,
                standardsListResponse.Standards.Single(s => s.LarsCode.ToString() == source.ProgrammeId).Level,
                Description = source.ShortDescription,
                LongDescription = source.Description,
                source.EmployerName,
                source.LiveDate,
                source.ProgrammeId,
                source.ProgrammeType,
                source.StartDate,
                IsPositiveAboutDisability = source.DisabilityConfident == DisabilityConfident.Yes,
                standardsListResponse.Standards.Single(s => s.LarsCode.ToString() == source.ProgrammeId).Route,
                EmployerLocation = new FindApprenticeshipJobs.Application.Shared.Address
                {
                    AddressLine1 = source.EmployerLocation?.AddressLine1,
                    AddressLine2 = source.EmployerLocation?.AddressLine2,
                    AddressLine3 = source.EmployerLocation?.AddressLine3,
                    AddressLine4 = source.EmployerLocation?.AddressLine4,
                    Postcode = source.EmployerLocation?.Postcode,
                    Latitude = source.EmployerLocation?.Latitude ?? 0,
                    Longitude = source.EmployerLocation?.Longitude ?? 0,
                },
                Wage = source.Wage == null ? null : new FindApprenticeshipJobs.Application.Shared.Wage
                {
                    Duration = source.Wage.Duration,
                    DurationUnit = source.Wage.DurationUnit,
                    FixedWageYearlyAmount = source.Wage.FixedWageYearlyAmount,
                    WageAdditionalInformation = source.Wage.WageAdditionalInformation,
                    WageType = source.Wage.WageType,
                    WeeklyHours = source.Wage.WeeklyHours,
                    WorkingWeekDescription = source.Wage.WorkingWeekDescription
                }
            };

            actual.Should().BeEquivalentTo(expectedResult);
        }


        private static GetStandardsListResponse SetupCoursesApiResponse(LiveVacancy vacancy)
        {
            var fixture = new Fixture();

            var larsCode = fixture.Create<int>();
            vacancy.ProgrammeId = larsCode.ToString();
            vacancy.ProgrammeType = "Standard";

            var standards = new List<GetStandardsListResponse.GetStandardsListItem>
            {
                new GetStandardsListResponse.GetStandardsListItem
                {
                    LarsCode = larsCode,
                    Level = fixture.Create<int>(),
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
