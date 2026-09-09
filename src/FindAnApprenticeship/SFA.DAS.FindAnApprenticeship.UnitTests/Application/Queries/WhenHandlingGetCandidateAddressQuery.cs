using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddress;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;
public class WhenHandlingGetCandidateAddressQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Address_From_Candidates(
        GetCandidateAddressQuery query,
        GetCandidateAddressApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        GetCandidateAddressQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetCandidateAddressApiResponse>(
                It.Is<GetCandidateAddressApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
    }
}
