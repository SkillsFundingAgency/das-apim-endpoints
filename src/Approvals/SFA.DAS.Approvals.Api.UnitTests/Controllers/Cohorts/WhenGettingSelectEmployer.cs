using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        bool useLearnerData,
        GetSelectEmployerQueryResult result,
        [Frozen] Mock<ISender> mediator,
        [Greedy] CohortController controller,
        CancellationToken cancellationToken)
    {
        mediator.Setup(x => x.Send(
            It.Is<GetSelectEmployerQuery>(c => 
                c.ProviderId == providerId &&
                c.SearchTerm == searchTerm &&
                c.SortField == sortField &&
                c.ReverseSort == reverseSort &&
                c.UseLearnerData == useLearnerData), cancellationToken)).ReturnsAsync(result);

        var actual = await controller.GetSelectEmployer(providerId, searchTerm, sortField, reverseSort, useLearnerData) as OkObjectResult;

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
        bool useLearnerData,
        [Frozen] Mock<ISender> mediator,
        [Greedy] CohortController controller,
        CancellationToken cancellationToken)
    {
        mediator.Setup(x => x.Send(
            It.IsAny<GetSelectEmployerQuery>(), cancellationToken)).ReturnsAsync((GetSelectEmployerQueryResult)null);

        var actual = await controller.GetSelectEmployer(providerId, searchTerm, sortField, reverseSort, useLearnerData);

        actual.Should().BeOfType<NotFoundResult>();
    }
}
