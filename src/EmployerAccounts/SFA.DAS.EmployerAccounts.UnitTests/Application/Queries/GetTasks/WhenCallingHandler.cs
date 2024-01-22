using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetTasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetTasks
{
    [TestFixture]
    public class WhenCallingHandler
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Tasks_Returns_GetTasksQueryResult(
          [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
          GetApprenticeshipUpdatesResponse cohortsResponse,
          GetTasksQuery request,
          GetTasksQueryHandler handler)
        {
            mockCommitmentsApi
                .Setup(m => m.Get<GetApprenticeshipUpdatesResponse>(It.Is<GetPendingApprenticeChangesRequest>(r => r.AccountId == request.AccountId)))
                .ReturnsAsync(cohortsResponse);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.AreEqual(3, result.NumberOfApprenticesToReview);
        }
    }
}