using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models.Cohorts;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetSelectEmployer;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Cohorts;

public class WhenGettingSelectEmployer
{
    [Test, MoqAutoData]
    public async Task Then_Get_Returns_Employers_From_Mediator(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        bool useLearnerData,
        [Frozen] Mock<ISender> mediator,
        [Greedy] CohortController controller)
    {
        // Arrange
        const int pageNumber = 1;
        const int pageSize = 50;

        var mediatorResult = new GetSelectEmployerQueryResult
        {
            AccountProviderLegalEntities =
                [new() { AccountLegalEntityName = "Test Entity", AccountName = "Test Account" }],
            Employers = ["Test Entity", "Test Account"],
            TotalCount = 1
        };

        mediator.Setup(x => x.Send(
                It.Is<GetSelectEmployerQuery>(q =>
                    q.ProviderId == providerId &&
                    q.SearchTerm == searchTerm &&
                    q.SortField == sortField &&
                    q.ReverseSort == reverseSort &&
                    q.PageNumber == pageNumber &&
                    q.PageSize == pageSize),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        // Act
        var controllerResult = await controller.GetSelectEmployer(providerId, searchTerm, sortField, reverseSort, useLearnerData, pageNumber, pageSize) as ObjectResult;

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
        bool useLearnerData,
        [Frozen] Mock<ISender> mediator,
        [Greedy] CohortController controller)
    {
        // Arrange
        GetSelectEmployerQueryResult nullResult = null;
        mediator.Setup(x => x.Send(
            It.IsAny<GetSelectEmployerQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(nullResult);

        // Act
        var controllerResult = await controller.GetSelectEmployer(providerId, searchTerm, sortField, reverseSort, useLearnerData, 1, 50);

        // Assert
        controllerResult.Should().BeOfType<NotFoundResult>();
    }

    [Test, MoqAutoData]
    public async Task And_PageSize_Over_100_Then_Returns_BadRequest(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        bool useLearnerData,
        [Greedy] CohortController controller)
    {
        // Act
        var controllerResult = await controller.GetSelectEmployer(providerId, searchTerm, sortField, reverseSort, useLearnerData, 1, 101);

        // Assert
        controllerResult.Should().BeOfType<BadRequestResult>();
    }

    [Test, MoqAutoData]
    public async Task And_PageNumber_Less_Than_1_Then_Returns_BadRequest(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        bool useLearnerData,
        [Greedy] CohortController controller)
    {
        // Act
        var controllerResult = await controller.GetSelectEmployer(providerId, searchTerm, sortField, reverseSort, useLearnerData, 0, 50);

        // Assert
        controllerResult.Should().BeOfType<BadRequestResult>();
    }

    [Test, MoqAutoData]
    public async Task And_PageSize_Less_Than_1_Then_Returns_BadRequest(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        bool useLearnerData,
        [Greedy] CohortController controller)
    {
        // Act
        var controllerResult = await controller.GetSelectEmployer(providerId, searchTerm, sortField, reverseSort, useLearnerData, 1, 0);

        // Assert
        controllerResult.Should().BeOfType<BadRequestResult>();
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_BadRequest(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        bool useLearnerData,
        [Frozen] Mock<ISender> mediator,
        [Frozen] Mock<ILogger<DraftApprenticeshipController>> logger,
        [Greedy] CohortController controller)
    {
        // Arrange
        mediator.Setup(x => x.Send(
            It.IsAny<GetSelectEmployerQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test exception"));

        // Act
        var controllerResult = await controller.GetSelectEmployer(providerId, searchTerm, sortField, reverseSort, useLearnerData, 1, 50);

        // Assert
        controllerResult.Should().BeOfType<BadRequestResult>();
    }
}

