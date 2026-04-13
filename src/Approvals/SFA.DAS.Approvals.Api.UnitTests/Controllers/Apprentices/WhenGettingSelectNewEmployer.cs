using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetSelectNewEmployer;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices;

public class WhenGettingSelectNewEmployer
{
    [Test, MoqAutoData]
    public async Task Then_Get_Returns_Employers_From_Mediator(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        int apprenticeshipId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApprenticesController controller)
    {
        // Arrange
        const int pageNumber = 1;
        const int pageSize = 50;

        var mediatorResult = new GetSelectNewEmployerQueryResult
        {
            AccountProviderLegalEntities =
                [new() { AccountLegalEntityName = "Test Entity", AccountName = "Test Account" }],
            Employers = ["Test Entity", "Test Account"],
            TotalCount = 1
        };

        mediator.Setup(x => x.Send(
                It.Is<GetSelectNewEmployerQuery>(q =>
                    q.ProviderId == providerId &&
                    q.ApprenticeshipId == apprenticeshipId &&
                    q.SearchTerm == searchTerm &&
                    q.SortField == sortField &&
                    q.ReverseSort == reverseSort &&
                    q.PageNumber == pageNumber &&
                    q.PageSize == pageSize),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        // Act
        var controllerResult = await controller.GetSelectEmployer(providerId, apprenticeshipId, searchTerm, sortField, reverseSort, false, pageNumber, pageSize) as ObjectResult;

        // Assert
        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetSelectEmployerResponse;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo((GetSelectEmployerResponse)mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Result_Is_Null_Then_Returns_NotFound(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        int apprenticeshipId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApprenticesController controller)
    {
        // Arrange
        GetSelectNewEmployerQueryResult nullResult = null;

        mediator.Setup(x => x.Send(It.IsAny<GetSelectNewEmployerQuery>(), CancellationToken.None))
            .ReturnsAsync(nullResult);

        // Act
        var controllerResult = await controller.GetSelectEmployer(providerId, apprenticeshipId, searchTerm, sortField, reverseSort, false, 1, 50);

        // Assert
        controllerResult.Should().BeOfType<NotFoundResult>();
    }

    [Test, MoqAutoData]
    public async Task And_PageSize_Over_100_Then_Returns_BadRequest(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        int apprenticeshipId,
        [Greedy] ApprenticesController controller)
    {
        // Act
        var controllerResult = await controller.GetSelectEmployer(providerId, apprenticeshipId, searchTerm, sortField, reverseSort, false, 1, 101);
        // Assert
        controllerResult.Should().BeOfType<BadRequestResult>();
    }

    [Test, MoqAutoData]
    public async Task And_PageNumber_Less_Than_1_Then_Returns_BadRequest(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        int apprenticeshipId,
        [Greedy] ApprenticesController controller)
    {
        // Act
        var controllerResult = await controller.GetSelectEmployer(providerId, apprenticeshipId, searchTerm, sortField, reverseSort, false, 0, 50);

        // Assert
        controllerResult.Should().BeOfType<BadRequestResult>();
    }

    [Test, MoqAutoData]
    public async Task And_PageSize_Less_Than_1_Then_Returns_BadRequest(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        int apprenticeshipId,
        [Greedy] ApprenticesController controller)
    {
        // Act
        var controllerResult = await controller.GetSelectEmployer(providerId, apprenticeshipId, searchTerm, sortField, reverseSort, false, 1, 0);

        // Assert
        controllerResult.Should().BeOfType<BadRequestResult>();
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_BadRequest(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        int apprenticeshipId,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ILogger<ApprenticesController>> logger,
        [Frozen] Mock<IMapper> mapper,
        [Greedy] ApprenticesController controller)
    {
        // Arrange
        mediator.Setup(x => x.Send(
                It.Is<GetSelectNewEmployerQuery>(q =>
                    q.ProviderId == providerId &&
                    q.ApprenticeshipId == apprenticeshipId &&
                    q.SearchTerm == searchTerm &&
                    q.SortField == sortField &&
                    q.ReverseSort == reverseSort &&
                    q.PageNumber == 1 &&
                    q.PageSize == 50), CancellationToken.None))
            .ThrowsAsync(new Exception("Test"));

        // Act
        var controllerResult = await controller.GetSelectEmployer(providerId, apprenticeshipId, searchTerm, sortField, reverseSort, false, 1, 50);

        // Assert
        controllerResult.Should().BeOfType<BadRequestResult>();
    }
}