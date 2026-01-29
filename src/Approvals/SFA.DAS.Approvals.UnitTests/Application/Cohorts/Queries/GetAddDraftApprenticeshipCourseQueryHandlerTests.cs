using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetEditApprenticeshipCourse;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipCourse;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts
{
    [TestFixture]
    public class GetAddDraftApprenticeshipCourseQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Standards_Are_Mapped_Correctly(
            GetAddDraftApprenticeshipCourseQuery query,
            GetProviderResponse provider,
            GetAccountLegalEntityResponse accountLegalEntity,
            ProviderStandardsData providerStandardsData,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
            [Frozen] Mock<IProviderStandardsService> providerStandardsService)
        {
            var serviceParameters = new ServiceParameters(Party.Provider, provider.ProviderId);

            var handler = new GetAddDraftApprenticeshipCourseQueryHandler(apiClient.Object,
                providerStandardsService.Object, serviceParameters);

            apiClient.Setup(x => x.GetWithResponseCode<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(() => new ApiResponse<GetProviderResponse>(provider, HttpStatusCode.OK, string.Empty));

            apiClient.Setup(x => x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(() => new ApiResponse<GetAccountLegalEntityResponse>(accountLegalEntity, HttpStatusCode.OK, string.Empty));

            providerStandardsService.Setup(x => x.GetStandardsData(provider.ProviderId))
                .ReturnsAsync(providerStandardsData);

            var standardsData = providerStandardsData.Standards.Select(x =>
                    new GetEditApprenticeshipCourseQueryResult.Standard
                    { CourseCode = x.CourseCode, Name = x.Name });

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standards.ToList().Should().BeEquivalentTo(standardsData);
        }

        [Test, MoqAutoData]
        public async Task IsMainProvider_Is_Mapped_Correctly(
            GetAddDraftApprenticeshipCourseQuery query,
            GetProviderResponse provider,
            GetAccountLegalEntityResponse accountLegalEntity,
            ProviderStandardsData providerStandardsData,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
            [Frozen] Mock<IProviderStandardsService> providerStandardsService)
        {
            var serviceParameters = new ServiceParameters(Party.Provider, provider.ProviderId);

            var handler = new GetAddDraftApprenticeshipCourseQueryHandler(apiClient.Object,
                providerStandardsService.Object, serviceParameters);

            apiClient.Setup(x => x.GetWithResponseCode<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(() => new ApiResponse<GetProviderResponse>(provider, HttpStatusCode.OK, string.Empty));

            apiClient.Setup(x => x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(() => new ApiResponse<GetAccountLegalEntityResponse>(accountLegalEntity, HttpStatusCode.OK, string.Empty));

            providerStandardsService.Setup(x => x.GetStandardsData(provider.ProviderId))
                .ReturnsAsync(providerStandardsData);

            var result = await handler.Handle(query, CancellationToken.None);

            result.IsMainProvider.Should().Be(providerStandardsData.IsMainProvider);
        }
    }
}
