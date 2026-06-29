using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Api.AppStart;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetApprenticeship;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries;

public class GetApprenticeshipsQueryHandlerTests
{
    private GetApprenticeshipQueryHandler _handler;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;

    private GetApprenticeshipResponse _apprenticeship;
    private GetApprenticeshipQuery _query;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();

        _apprenticeship = fixture.Build<GetApprenticeshipResponse>()
            .Create();

        _query = fixture.Create<GetApprenticeshipQuery>();

        _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipResponse>(It.Is<GetApprenticeshipRequest>(
                    r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(_apprenticeship, HttpStatusCode.OK, string.Empty));

        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mappingConfig.CreateMapper();

        _handler = new GetApprenticeshipQueryHandler(_apiClient.Object, mapper);
    }

    [Test]
    public async Task Handle_when_apprenticeship_is_returned()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_apprenticeship);
    }

    [Test]
    public async Task Handle_No_apprenticeship_returned()
    {
        _apiClient.Setup(x =>
              x.GetWithResponseCode<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
          .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(null, HttpStatusCode.NotFound, string.Empty));
        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Should().BeNull();
    }
}