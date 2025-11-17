using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Learning.Api.Controllers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.Learning.Api.UnitTests.Controllers.CollectionCalendar;

[TestFixture]
public class WhenGettingAcademicYear
{
    [Test, MoqAutoData]
    public async Task Then_Gets_ApprenticeshipKey_From_ApiClient(
        GetAcademicYearsResponse expectedResponse,
        DateTime searchDate,
        Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> mockCollectionCalendarApiClient)
    {
        //  Arrange
        mockCollectionCalendarApiClient.Setup(x => x.Get<GetAcademicYearsResponse>(It.IsAny<GetAcademicYearByDateRequest>())).ReturnsAsync(expectedResponse);

        var controller = new CollectionCalendarController(mockCollectionCalendarApiClient.Object);

        //  Act
        var result = await controller.GetAcademicYear(searchDate);

        //  Assert
        var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
        var actualResponse = okObjectResult.Value.ShouldBeOfType<GetAcademicYearsResponse>();
        actualResponse.Should().Be(expectedResponse);
    }
}
