using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.InvalidIlrChanges.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;

namespace SFA.DAS.Approvals.UnitTests.Application.InvalidIlrChanges.Queries;

public class GetInvalidIlrChangesQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ThenAggregatesLearnerNameWithInnerRequestSets(
        GetInvalidIlrChangesQuery query,
        GetApprenticeshipResponse apprenticeship,
        GetInvalidIlrChangesResponse innerResponse,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient)
    {
        apprenticeship.ProviderId = query.ProviderId;
        apprenticeship.EmployerAccountId = 123;
        apprenticeship.FirstName = "Jane";
        apprenticeship.LastName = "Doe";

        apiClient.Setup(x => x.GetWithResponseCode<GetApprenticeshipResponse>(
                It.Is<GetApprenticeshipRequest>(request => request.ApprenticeshipId == query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(apprenticeship, HttpStatusCode.OK, string.Empty));

        apiClient.Setup(x => x.GetWithResponseCode<GetInvalidIlrChangesResponse>(
                It.Is<GetInvalidIlrChangesRequest>(request =>
                    request.ApprenticeshipId == query.ApprenticeshipId &&
                    request.ProviderId == query.ProviderId)))
            .ReturnsAsync(new ApiResponse<GetInvalidIlrChangesResponse>(innerResponse, HttpStatusCode.OK, string.Empty));

        var handler = new GetInvalidIlrChangesQueryHandler(apiClient.Object, new ServiceParameters(Party.Provider, query.ProviderId));

        var result = await handler.Handle(query, CancellationToken.None);

        result.FirstName.Should().Be("Jane");
        result.LastName.Should().Be("Doe");
        result.RequestSets.Should().BeEquivalentTo(innerResponse.RequestSets);
    }

    [Test, MoqAutoData]
    public async Task Handle_ThenReturnsNullWhenApprenticeshipIsMissing(
        GetInvalidIlrChangesQuery query,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient)
    {
        apiClient.Setup(x => x.GetWithResponseCode<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(null, HttpStatusCode.NotFound, string.Empty));

        var handler = new GetInvalidIlrChangesQueryHandler(apiClient.Object, new ServiceParameters(Party.Provider, query.ProviderId));

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }
}
