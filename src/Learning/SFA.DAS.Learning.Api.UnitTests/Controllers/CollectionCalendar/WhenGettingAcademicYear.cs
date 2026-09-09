using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.CollectionCalendar.Contracts.ApiRequests;
using SFA.DAS.CollectionCalendar.Contracts.ApiResponses;
using SFA.DAS.CollectionCalendar.Contracts.Client;
using SFA.DAS.Learning.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.Learning.Api.UnitTests.Controllers.CollectionCalendar;

[TestFixture]
public class WhenGettingAcademicYear
{
    [Test, MoqAutoData]
    public async Task Then_Gets_ApprenticeshipKey_From_ApiClient(
        AcademicYearDetails expectedResponse,
        DateTime searchDate,
        Mock<ICollectionCalendarClient<CollectionCalendarConfiguration>> mockCollectionCalendarClient)
    {
        //  Arrange
        mockCollectionCalendarClient.Setup(x => x.Get<AcademicYearDetails>(It.IsAny<GetAcademicyearsApiRequest>())).ReturnsAsync(expectedResponse);

        var controller = new CollectionCalendarController(mockCollectionCalendarClient.Object);

        //  Act
        var result = await controller.GetAcademicYear(searchDate);

        //  Assert
        var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
        var actualResponse = okObjectResult.Value.ShouldBeOfType<AcademicYearDetails>();
        actualResponse.Should().Be(expectedResponse);
    }
}
