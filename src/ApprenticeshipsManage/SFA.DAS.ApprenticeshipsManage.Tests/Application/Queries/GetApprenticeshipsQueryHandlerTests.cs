using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeshipsManage.Application.Queries.GetApprenticeships;
using SFA.DAS.ApprenticeshipsManage.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeshipsManage.Tests.Application.Queries;
public class GetApprenticeshipsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Users_With_Matching_Emails(
        GetApprenticeshipsQuery query,
        PagedApprenticeshipsResponse apiResponse,
        [Frozen] Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> apiClient,
        [Frozen] Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> calendarApi,
        GetApprenticeshipsQueryHandler sut)
    {
        var calendarResponse = new GetAcademicYearsResponse
        {
            StartDate = new DateTime(2024, 8, 1),
            EndDate = new DateTime(2025, 7, 31)
        };

        query.AcademicYearDate = new DateTime(2024, 11, 30);

        var expectedUrl = $"/{query.Ukprn}/apprenticeships/by-dates?startDate={calendarResponse.StartDate.ToString("yyy-MM-dd")}&endDate={calendarResponse.EndDate.ToString("yyy-MM-dd")}&page={query.Page}&pageSize={query.PageSize}";

        calendarApi.Setup(client => client.Get<GetAcademicYearsResponse>(It.IsAny<GetAcademicYearByDateRequest>()))
          .ReturnsAsync(calendarResponse);

        apiClient.Setup(client => client.Get<PagedApprenticeshipsResponse>(It.Is<GetAllApprenticeshipsByDatesRequest>(c => c.GetUrl == expectedUrl)))
            .ReturnsAsync(apiResponse);

        var actual = await sut.Handle(query, It.IsAny<CancellationToken>());

        actual.Items.Should().BeEquivalentTo(apiResponse.Items);
        actual.TotalItems.Should().Be(apiResponse.TotalItems);
        actual.TotalPages.Should().Be((int)Math.Ceiling((double)apiResponse.TotalItems / apiResponse.PageSize));
        actual.Page.Should().Be(apiResponse.Page);
        actual.PageSize.Should().Be(apiResponse.PageSize);

        //apiClient.Verify(client => client.GenerateServiceToken("ApprenticeshipsManage"), Times.Once());
    }
}
