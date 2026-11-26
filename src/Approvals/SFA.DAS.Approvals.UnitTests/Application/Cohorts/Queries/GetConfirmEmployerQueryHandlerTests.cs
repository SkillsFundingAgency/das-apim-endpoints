using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetConfirmEmployer;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts
{
    [TestFixture]
    public class GetConfirmEmployerQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task When_CallingParty_Have_DeclaredStandards__Then_Result_Is_False(
            GetConfirmEmployerQuery query,
            GetProviderResponse providerResponse,
            ProviderStandardsData providerStandardsData,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
            [Frozen] Mock<IProviderCoursesOrStandardsService> providerStandardsService)
        {
            var serviceParameters = new ServiceParameters(Party.Provider, providerResponse.ProviderId);

            var handler = new GetConfirmEmployerQueryHandler(apiClient.Object,
                providerStandardsService.Object, serviceParameters);

            apiClient.Setup(x => x.GetWithResponseCode<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(() => new ApiResponse<GetProviderResponse>(providerResponse, HttpStatusCode.OK, string.Empty));

            providerStandardsService.Setup(x => x.GetCoursesData(providerResponse.ProviderId))
                .ReturnsAsync(providerStandardsData);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result.HasNoDeclaredStandards, Is.False);
        }

        [Test, MoqAutoData]
        public async Task When_CallingParty_Does_Not_Have_DeclaredStandards__Then_Result_Is_True(
            GetConfirmEmployerQuery query,
            GetProviderResponse providerResponse,
            ProviderStandardsData providerStandardsData,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
            [Frozen] Mock<IProviderCoursesOrStandardsService> providerStandardsService)
        {
            var serviceParameters = new ServiceParameters(Party.Provider, providerResponse.ProviderId);

            var handler = new GetConfirmEmployerQueryHandler(apiClient.Object,
                providerStandardsService.Object, serviceParameters);

            apiClient.Setup(x => x.GetWithResponseCode<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(() => new ApiResponse<GetProviderResponse>(providerResponse, HttpStatusCode.OK, string.Empty));

            providerStandardsData.Standards = null;

            providerStandardsService.Setup(x => x.GetCoursesData(providerResponse.ProviderId))
                .ReturnsAsync(providerStandardsData);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result.HasNoDeclaredStandards, Is.True);
        }


        [Test, MoqAutoData]
        public async Task When_Provider_Is_Not_Found_Then_Result_Is_Null(
            GetConfirmEmployerQuery query,
            GetProviderResponse providerResponse,
            ProviderStandardsData providerStandardsData,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
            [Frozen] Mock<IProviderCoursesOrStandardsService> providerStandardsService)
        {
            var serviceParameters = new ServiceParameters(Party.Provider, providerResponse.ProviderId);

            var handler = new GetConfirmEmployerQueryHandler(apiClient.Object,
                providerStandardsService.Object, serviceParameters);

            apiClient.Setup(x => x.GetWithResponseCode<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(() => new ApiResponse<GetProviderResponse>(null, HttpStatusCode.OK, string.Empty));

            providerStandardsService.Setup(x => x.GetCoursesData(providerResponse.ProviderId))
                .ReturnsAsync(providerStandardsData);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result, Is.Null);
        }
    }
}
