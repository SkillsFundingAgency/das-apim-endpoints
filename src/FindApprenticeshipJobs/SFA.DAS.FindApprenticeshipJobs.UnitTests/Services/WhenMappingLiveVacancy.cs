using AutoFixture;
using SFA.DAS.FindApprenticeshipJobs.Application.Shared;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using LiveVacancy = SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses.LiveVacancy;
using Qualification = SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses.Qualification;

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
        
        [Test]
        [MoqInlineAutoData(null, "NonNational")]
        [MoqInlineAutoData(AvailableWhere.OneLocation, "NonNational")]
        [MoqInlineAutoData(AvailableWhere.MultipleLocations, "NonNational")]
        [MoqInlineAutoData(AvailableWhere.AcrossEngland, "National")]
        public void Then_The_Mapped_Vacancy_Has_The_Correct_VacancyLocationType(
            AvailableWhere? locationType,
            string expectedLocationType,
            LiveVacancy source,
            [Frozen] Mock<ICourseService> courseService,
            LiveVacancyMapper sut)
        {
            // arrange
            source.EmployerLocationOption = locationType;
            var mockStandardsListResponse =  SetupCoursesApiResponse(source);

            // act
            var result = sut.Map(source, mockStandardsListResponse);

            // assert
            result.VacancyLocationType.Should().Be(expectedLocationType);
        }

        [Test, MoqAutoData]
        public void Then_The_Nhs_Vacancy_Is_Mapped(GetNhsJobApiDetailResponse source, LiveVacancyMapper liveVacancyMapper, DateTime closeDate, DateTime postDate, GetLocationsListItem address, string address1, string postCode1, string postCode2, GetRoutesListItem route)
        {
            address.Postcode = $"{postCode1} {postCode2}";
            var addressResponse = new GetLocationsListResponse
            {
                Locations = new List<GetLocationsListItem>
                {
                    address
                }
            };
            source.CloseDate = closeDate.ToString();
            source.PostDate = postDate.ToString();
            source.Locations.Clear();
            source.Locations.Add(new GetNhsJobLocationApiResponse{Location = $"{address1}, {postCode1}{postCode2} "});
            
            
            var actual = liveVacancyMapper.Map(source, addressResponse, route);

            actual.Id.Should().Be(source.Id);
            actual.Title.Should().Be(source.Title);
            actual.Description.Should().Be(source.Description);
            actual.ClosingDate.Should().BeCloseTo(closeDate, TimeSpan.FromHours(1));
            actual.PostedDate.Should().BeCloseTo(postDate, TimeSpan.FromHours(1));
            actual.EmployerName.Should().Be(source.Employer);
            actual.VacancyReference.Should().Be(source.Reference);
            actual.ApplicationUrl.Should().Be(source.Url);
            actual.Wage.WageText.Should().Be(source.Salary);
            actual.Address.AddressLine4.Should().Be(address1);
            actual.Address.Postcode.Should().Be($"{postCode1}{postCode2}");
            actual.Address.Latitude.Should().Be(address.Location.GeoPoint.FirstOrDefault());
            actual.Address.Longitude.Should().Be(address.Location.GeoPoint.LastOrDefault());
            actual.Address.Country.Should().Be(address.Country);
            actual.Route.Should().Be(route.Name);
            actual.RouteCode.Should().Be(route.Id);
            actual.SearchTags.Should().Be("NHS National Health Service Health Medical Hospital");
            actual.EmploymentLocations.Should().BeNullOrEmpty();
        }

        private static void AssertResponse(FindApprenticeshipJobs.Application.Shared.LiveVacancy actual, LiveVacancy source, GetStandardsListResponse standardsListResponse)
        {
            var expectedResult = new
            {
                Id = source.VacancyReference.ToString(),
                VacancyReference = source.VacancyReference.ToString(),
                VacancyId = source.Id,
                source.Title,
                PostedDate = source.LiveDate,
                source.StartDate,
                source.ClosingDate,
                Description = source.ShortDescription,
                source.NumberOfPositions,
                source.EmployerName,
                source.ApplicationMethod,
                source.ApplicationUrl,
                ProviderName = source.TrainingProvider.Name,
                source.TrainingProvider.Ukprn,
                IsPositiveAboutDisability = false,
                source.EmployerContactName,
                source.EmployerContactEmail,
                source.EmployerContactPhone,
                IsEmployerAnonymous = source.IsAnonymous,
                VacancyLocationType = "NonNational",
                ApprenticeshipLevel = "Higher",
                Wage = new FindApprenticeshipJobs.Application.Shared.Wage
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
                    Over25NationalMinimumWage = source.Wage.Over25NationalMinimumWage,
                    WageText = source.Wage.WageText,
                    CompanyBenefitsInformation = source.Wage.CompanyBenefitsInformation
                },
                AnonymousEmployerName = source.IsAnonymous ? source.EmployerName: null,
                IsDisabilityConfident = source.DisabilityConfident,
                source.AccountId,
                source.AccountLegalEntityId,
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
                source.Address,
                source.OtherAddresses,
                source.EmployerLocations,
                source.EmployerLocationInformation,
                source.EmployerLocationOption,
                
                source.AdditionalQuestion1,
                source.AdditionalQuestion2,
                source.AdditionalTrainingDescription
            };

            actual.Should().BeEquivalentTo(expectedResult, options => options
                .WithMapping("EmployerLocations", "EmploymentLocations")
                .WithMapping("EmployerLocationOption", "EmploymentLocationOption")
                .WithMapping("EmployerLocationInformation", "EmploymentLocationInformation")
            );
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
