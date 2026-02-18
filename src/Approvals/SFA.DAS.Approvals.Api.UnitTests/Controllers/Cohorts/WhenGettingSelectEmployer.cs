using System;
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
    public async Task Then_Mediator_Query_Is_Handled_And_Response_Returned(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        [Frozen] Mock<ISender> mediator,
        [Greedy] CohortController controller)
    {
        // Arrange
        var result = new GetSelectEmployerQueryResult
        {
            AccountProviderLegalEntities =
                [new() { AccountLegalEntityName = "Test Entity", AccountName = "Test Account" }],
            Employers = ["Test Entity", "Test Account"]
        };

        mediator.Setup(x => x.Send(
            It.Is<GetSelectEmployerQuery>(c => 
                c.ProviderId == providerId &&
                c.SearchTerm == searchTerm &&
                c.SortField == sortField &&
                c.ReverseSort == reverseSort), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        // Act
        var actual = await controller.GetSelectEmployer(providerId, searchTerm, sortField, reverseSort) as OkObjectResult;

        // Assert
        actual.Should().NotBeNull();
        var actualModel = actual.Value as GetSelectEmployerResponse;
        actualModel.Should().NotBeNull();
        actualModel.AccountProviderLegalEntities.Should().BeEquivalentTo(result.AccountProviderLegalEntities);
        actualModel.Employers.Should().BeEquivalentTo(result.Employers);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_NotFound_When_Result_Is_Null(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        [Frozen] Mock<ISender> mediator,
        [Greedy] CohortController controller)
    {
        // Arrange
        GetSelectEmployerQueryResult nullResult = null;
        mediator.Setup(x => x.Send(
            It.IsAny<GetSelectEmployerQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(nullResult);

        // Act
        var actual = await controller.GetSelectEmployer(providerId, searchTerm, sortField, reverseSort);

        // Assert
        actual.Should().BeOfType<NotFoundResult>();
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_BadRequest_When_Exception_Is_Thrown(
        int providerId,
        string searchTerm,
        string sortField,
        bool reverseSort,
        [Frozen] Mock<ISender> mediator,
        [Frozen] Mock<ILogger<DraftApprenticeshipController>> logger,
        [Greedy] CohortController controller)
    {
        // Arrange
        mediator.Setup(x => x.Send(
            It.IsAny<GetSelectEmployerQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test exception"));

        // Act
        var actual = await controller.GetSelectEmployer(providerId, searchTerm, sortField, reverseSort);

        // Assert
        actual.Should().BeOfType<BadRequestResult>();
    }
}

