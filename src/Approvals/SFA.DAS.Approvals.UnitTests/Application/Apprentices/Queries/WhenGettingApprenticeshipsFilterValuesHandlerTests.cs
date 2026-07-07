using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Api.AppStart;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetApprenticeshipsFilterValues;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries;

public class WhenGettingApprenticeshipsFilterValuesHandlerTests
{
    private GetApprenticeshipsFilterValuesQueryHandler _handler;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;

    private GetApprenticeshipsFilterValuesResponse _apprenticeshipsFilters;
    private long _providerId;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();

        _apprenticeshipsFilters = fixture.Build<GetApprenticeshipsFilterValuesResponse>()
            .Create();

        _providerId = fixture.Create<long>();

        _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipsFilterValuesResponse>(It.Is<GetApprenticeshipsFilterValuesRequest>(
                    r => r.ProviderId == _providerId

                    )))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipsFilterValuesResponse>(_apprenticeshipsFilters, HttpStatusCode.OK, string.Empty));
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mappingConfig.CreateMapper();

        _handler = new GetApprenticeshipsFilterValuesQueryHandler(_apiClient.Object, mapper);
    }

    [Test]
    public async Task Handle_when_apprenticeships_filters_are_returned()
    {
        var result = await _handler.Handle(new GetApprenticeshipsFilterValuesQuery { ProviderId = _providerId }, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_apprenticeshipsFilters);
    }

    [Test]
    public async Task Handle_No_apprenticeships_filters_returned()
    {
        _apiClient.Setup(x =>
              x.GetWithResponseCode<GetApprenticeshipsFilterValuesResponse>(It.IsAny<GetApprenticeshipsFilterValuesRequest>()))
          .ReturnsAsync(new ApiResponse<GetApprenticeshipsFilterValuesResponse>(null, HttpStatusCode.NotFound, string.Empty));

        var result = await _handler.Handle(new GetApprenticeshipsFilterValuesQuery { ProviderId = _providerId }, CancellationToken.None);

        result.Should().BeNull();
    }
}