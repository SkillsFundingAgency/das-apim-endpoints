using System.Linq;
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
          GetCohortsResponse cohortsResponse,
          GetTasksQuery request,
          GetTasksQueryHandler handler)
        {
            // Arrange
            foreach (var cohort in cohortsResponse.Cohorts)
            {
                cohort.WithParty = Party.Employer;
                cohort.IsDraft = false;
            }

            mockCommitmentsApi
                .Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
                .ReturnsAsync(cohortsResponse);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.AreEqual(3, result.NumberOfCohortsReadyToReview);
        }


        [Test, MoqAutoData]
        public async Task Then_Only_Returns_Pending_Employer_Cohorts(
          [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
          GetCohortsResponse cohortsResponse,
          GetTasksQuery request,
          GetTasksQueryHandler handler)
        {
            cohortsResponse.Cohorts.Select((cohort, index) =>
            {
                cohort.WithParty = index == 0 ? Party.Provider : Party.Employer;
                cohort.IsDraft = index == 0;
                return cohort;
            }).ToArray();

            mockCommitmentsApi
                .Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
                .ReturnsAsync(cohortsResponse);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            Assert.AreEqual(2, result.NumberOfCohortsReadyToReview);
        }


        [Test, MoqAutoData]
        public async Task When_Cohorts_Are_Null_Return_Zero(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
        {
            mockCommitmentsApi
                .Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
                .ReturnsAsync(new GetCohortsResponse());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.AreEqual(0, result.NumberTransferPledgeApplicationsToReview);
        }
    }
}