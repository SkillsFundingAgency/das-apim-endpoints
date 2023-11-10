using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;
public class WhenHandlingGetLiveVacancies
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Live_Vacancies(
        GetLiveVacanciesQuery mockQuery,
        ApiResponse<GetLiveVacanciesApiResponse> mockApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockApiClient,
        GetLiveVacanciesQueryHandler sut)
    {
        mockApiClient.Setup(client => client.GetWithResponseCode<GetLiveVacanciesApiResponse>(It.IsAny<GetLiveVacanciesApiRequest>())).ReturnsAsync(mockApiResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        AssertResponse(actual, mockApiResponse.Body);
    }

    [Test, MoqAutoData]
    public async Task And_Api_Client_Returns_Null(
        GetLiveVacanciesQuery mockQuery,
        ApiResponse<GetLiveVacanciesApiResponse> mockApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockApiClient,
        GetLiveVacanciesQueryHandler sut)
    {
        mockApiResponse.Body.Vacancies = Enumerable.Empty<LiveVacancy>();
        mockApiClient.Setup(client => client.GetWithResponseCode<GetLiveVacanciesApiResponse>(It.IsAny<GetLiveVacanciesApiRequest>())).ReturnsAsync(mockApiResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        AssertResponse(actual, mockApiResponse.Body);
    }

    private static void AssertResponse(GetLiveVacanciesQueryResult actual, GetLiveVacanciesApiResponse mockApiResponse)
    {
        var expectedVacancies = mockApiResponse.Vacancies.Select(x => new
        {
            x.VacancyId,
            VacancyTitle = x.Title,
            ApprenticeshipTitle = x.Title, //todo: to change
            x.Description,
            x.EmployerName,
            x.LiveDate,
            x.ProgrammeId,
            x.ProgrammeType,
            x.StartDate,
            x.RouteId, //todo: to change
            EmployerLocation = new GetLiveVacanciesQueryResult.Address
            {
                AddressLine1 = x.EmployerLocation?.AddressLine1,
                AddressLine2 = x.EmployerLocation?.AddressLine2,
                AddressLine3 = x.EmployerLocation?.AddressLine3,
                AddressLine4 = x.EmployerLocation?.AddressLine4,
                Postcode = x.EmployerLocation?.Postcode,
                Latitude = x.EmployerLocation?.Latitude ?? 0,
                Longitude = x.EmployerLocation?.Longitude ?? 0,
            },
            Wage = x.Wage == null ? null : new GetLiveVacanciesQueryResult.Wage
            {
                Duration = x.Wage.Duration,
                DurationUnit = x.Wage.DurationUnit,
                FixedWageYearlyAmount = x.Wage.FixedWageYearlyAmount,
                WageAdditionalInformation = x.Wage.WageAdditionalInformation,
                WageType = x.Wage.WageType,
                WeeklyHours = x.Wage.WeeklyHours,
                WorkingWeekDescription = x.Wage.WorkingWeekDescription
            }
        });

        actual.Should().BeOfType<GetLiveVacanciesQueryResult>();
        actual.PageSize.Should().Be(mockApiResponse.PageSize);
        actual.PageNo.Should().Be(mockApiResponse.PageNo);
        actual.TotalLiveVacanciesReturned.Should().Be(mockApiResponse.TotalLiveVacanciesReturned);
        actual.TotalLiveVacancies.Should().Be(mockApiResponse.TotalLiveVacancies);
        actual.TotalPages.Should().Be(mockApiResponse.TotalPages);
        actual.Vacancies.Should().BeEquivalentTo(expectedVacancies);
    }
}
