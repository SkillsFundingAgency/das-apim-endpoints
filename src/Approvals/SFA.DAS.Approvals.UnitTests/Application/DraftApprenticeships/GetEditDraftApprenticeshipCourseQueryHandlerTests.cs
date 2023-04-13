using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions.Common;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipCourse;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

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
            [Frozen] Mock<IProviderStandardsService> providerStandardsService,
            GetEditDraftApprenticeshipCourseQueryHandler handler)
        {
            apiClient.Setup(x => x.Get<GetCohortResponse>(It.Is<GetCohortRequest>(r => r.CohortId == query.CohortId)))
                .ReturnsAsync(cohort);
            
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
            [Frozen] Mock<IProviderStandardsService> providerStandardsService,
            GetEditDraftApprenticeshipCourseQueryHandler handler)
        {
            apiClient.Setup(x => x.Get<GetCohortResponse>(It.Is<GetCohortRequest>(r => r.CohortId == query.CohortId)))
                .ReturnsAsync(cohort);

            providerStandardsService.Setup(x => x.GetStandardsData(cohort.ProviderId))
                .ReturnsAsync(providerStandardsData);

            var result = await handler.Handle(query, CancellationToken.None);

            result.IsMainProvider.IsSameOrEqualTo(providerStandardsData.IsMainProvider);
        }
    }
}
