using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetHasDeclaredStandards;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts
{
    [TestFixture]
    public class GetHasDeclaredStandardsQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task When_CallingParty_Have_DeclaredStandards__Then_Result_Is_False(
            GetHasDeclaredStandardsQuery query,
            GetProviderResponse providerResponse,
            ProviderStandardsData providerStandardsData,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
            [Frozen] Mock<IProviderStandardsService> providerStandardsService)
        {
            var serviceParameters = new ServiceParameters(Party.Provider, providerResponse.ProviderId);

            var handler = new GetHasDeclaredStandardsQueryHandler(apiClient.Object,
                providerStandardsService.Object, serviceParameters);

            apiClient.Setup(x => x.GetWithResponseCode<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(() => new ApiResponse<GetProviderResponse>(providerResponse, HttpStatusCode.OK, string.Empty));

            providerStandardsService.Setup(x => x.GetStandardsData(providerResponse.ProviderId))
                .ReturnsAsync(providerStandardsData);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.IsFalse(result.HasNoDeclaredStandards);
        }

        [Test, MoqAutoData]
        public async Task When_CallingParty_Does_Not_Have_DeclaredStandards__Then_Result_Is_True(
            GetHasDeclaredStandardsQuery query,
            GetProviderResponse providerResponse,
            ProviderStandardsData providerStandardsData,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
            [Frozen] Mock<IProviderStandardsService> providerStandardsService)
        {
            var serviceParameters = new ServiceParameters(Party.Provider, providerResponse.ProviderId);

            var handler = new GetHasDeclaredStandardsQueryHandler(apiClient.Object,
                providerStandardsService.Object, serviceParameters);

            apiClient.Setup(x => x.GetWithResponseCode<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(() => new ApiResponse<GetProviderResponse>(providerResponse, HttpStatusCode.OK, string.Empty));

            providerStandardsData.Standards = null;

            providerStandardsService.Setup(x => x.GetStandardsData(providerResponse.ProviderId))
                .ReturnsAsync(providerStandardsData);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.IsTrue(result.HasNoDeclaredStandards);
        }


        [Test, MoqAutoData]
        public async Task When_Provider_Is_Not_Found_Then_Result_Is_Null(
            GetHasDeclaredStandardsQuery query,
            GetProviderResponse providerResponse,
            ProviderStandardsData providerStandardsData,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
            [Frozen] Mock<IProviderStandardsService> providerStandardsService)
        {
            var serviceParameters = new ServiceParameters(Party.Provider, providerResponse.ProviderId);

            var handler = new GetHasDeclaredStandardsQueryHandler(apiClient.Object,
                providerStandardsService.Object, serviceParameters);

            apiClient.Setup(x => x.GetWithResponseCode<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(() => new ApiResponse<GetProviderResponse>(null, HttpStatusCode.OK, string.Empty));

            providerStandardsService.Setup(x => x.GetStandardsData(providerResponse.ProviderId))
                .ReturnsAsync(providerStandardsData);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.IsNull(result);
        }
    }
}
