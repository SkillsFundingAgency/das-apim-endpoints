using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships
{
    public class WhenGettingDraftApprenticeshipsForACohort
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_The_DraftApprenticeships_Are_Returned(
          GetDraftApprenticeshipsQuery query,
          GetDraftApprenticeshipsResponse apiResponse,
          [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
          GetDraftApprenticeshipsQueryHandler handler
          )
        {
            apiClient.Setup(x => x.Get<GetDraftApprenticeshipsResponse>(It.Is<GetDraftApprenticeshipsRequest>(x => x.CohortId == query.CohortId))).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.DraftApprenticeships.Should().BeEquivalentTo(apiResponse.DraftApprenticeships.Select(item => (Approvals.Application.DraftApprenticeships.Queries.DraftApprenticeship)item));
        }
    }
}
