using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.SelectProvider.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.SelectProvider;

public class WhenGettingSelectProvider
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_To_Get_LegalEntity_Returns_Name(
       GetSelectProviderQuery query,
       GetAccountLegalEntityResponse ale,
       [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
       GetSelectProviderQueryHandler handler
       )
    {
        apiClient.Setup(x => 
            x.Get<GetAccountLegalEntityResponse>(It.Is<GetAccountLegalEntityRequest>(x => x.AccountLegalEntityId == query.AccountLegalEntityId)))
            .ReturnsAsync(ale);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.AccountLegalEntity.LegalEntityName.Should().Be(ale.LegalEntityName);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_The_AccountLegalEntity_Is_Returned(
        GetSelectProviderQuery query,
        GetProvidersResponse providers,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
        GetSelectProviderQueryHandler handler
    )
    {
        apiClient.Setup(x =>
                x.Get<GetProvidersResponse>(It.IsAny<GetProvidersRequest>()))
            .ReturnsAsync(providers);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Providers.Should().BeEquivalentTo(providers.Providers);
    }
}

