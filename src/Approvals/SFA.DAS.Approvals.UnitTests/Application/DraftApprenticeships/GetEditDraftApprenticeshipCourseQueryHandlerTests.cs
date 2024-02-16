using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions.Common;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipCourse;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships
{
    [TestFixture]
    public class GetEditDraftApprenticeshipCourseQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Standards_Are_Mapped_Correctly(
            GetEditDraftApprenticeshipCourseQuery query,
            GetCohortResponse cohort,
            ProviderStandardsData providerStandardsData,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
            [Frozen] Mock<IProviderStandardsService> providerStandardsService)
        {
            var serviceParameters = new ServiceParameters(Party.Provider, cohort.ProviderId);

            var handler = new GetEditDraftApprenticeshipCourseQueryHandler(apiClient.Object,
                providerStandardsService.Object, serviceParameters);

            apiClient.Setup(x => x.GetWithResponseCode<GetCohortResponse>(It.Is<GetCohortRequest>(r => r.CohortId == query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetCohortResponse>(cohort, HttpStatusCode.OK, string.Empty));
            
            providerStandardsService.Setup(x => x.GetStandardsData(cohort.ProviderId))
                .ReturnsAsync(providerStandardsData);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standards.ToList().IsSameOrEqualTo(providerStandardsData.Standards.ToList());
        }

        [Test, MoqAutoData]
        public async Task IsMainProvider_Is_Mapped_Correctly(
            GetEditDraftApprenticeshipCourseQuery query,
            GetCohortResponse cohort,
            ProviderStandardsData providerStandardsData,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
            [Frozen] Mock<IProviderStandardsService> providerStandardsService)
        {
            var serviceParameters = new ServiceParameters(Party.Provider, cohort.ProviderId);

            var handler = new GetEditDraftApprenticeshipCourseQueryHandler(apiClient.Object,
                providerStandardsService.Object, serviceParameters);

            apiClient.Setup(x => x.GetWithResponseCode<GetCohortResponse>(It.Is<GetCohortRequest>(r => r.CohortId == query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetCohortResponse>(cohort, HttpStatusCode.OK, string.Empty));

            providerStandardsService.Setup(x => x.GetStandardsData(cohort.ProviderId))
                .ReturnsAsync(providerStandardsData);

            var result = await handler.Handle(query, CancellationToken.None);

            result.IsMainProvider.IsSameOrEqualTo(providerStandardsData.IsMainProvider);
        }

        [Test, MoqAutoData]
        public async Task When_CallingParty_Does_Not_Own_Cohort_Then_Result_Is_Null(
            GetEditDraftApprenticeshipCourseQuery query,
            GetCohortResponse cohort,
            ProviderStandardsData providerStandardsData,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
            [Frozen] Mock<IProviderStandardsService> providerStandardsService)
        {
            var serviceParameters = new ServiceParameters(Party.Provider, cohort.ProviderId + 1);

            var handler = new GetEditDraftApprenticeshipCourseQueryHandler(apiClient.Object,
                providerStandardsService.Object, serviceParameters);

            apiClient.Setup(x => x.GetWithResponseCode<GetCohortResponse>(It.Is<GetCohortRequest>(r => r.CohortId == query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetCohortResponse>(cohort, HttpStatusCode.OK, string.Empty));

            providerStandardsService.Setup(x => x.GetStandardsData(cohort.ProviderId))
                .ReturnsAsync(providerStandardsData);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result, Is.Null);
        }

        [Test, MoqAutoData]
        public async Task When_Cohort_Is_Not_Found_Then_Result_Is_Null(
            GetEditDraftApprenticeshipCourseQuery query,
            ProviderStandardsData providerStandardsData,
            long providerId,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
            [Frozen] Mock<IProviderStandardsService> providerStandardsService)
        {
            var serviceParameters = new ServiceParameters(Party.Provider, providerId);

            var handler = new GetEditDraftApprenticeshipCourseQueryHandler(apiClient.Object,
                providerStandardsService.Object, serviceParameters);

            apiClient.Setup(x => x.GetWithResponseCode<GetCohortResponse>(It.Is<GetCohortRequest>(r => r.CohortId == query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetCohortResponse>(null, HttpStatusCode.NotFound, "Test - not found"));

            providerStandardsService.Setup(x => x.GetStandardsData(providerId))
                .ReturnsAsync(providerStandardsData);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result, Is.Null);
        }
    }
}
