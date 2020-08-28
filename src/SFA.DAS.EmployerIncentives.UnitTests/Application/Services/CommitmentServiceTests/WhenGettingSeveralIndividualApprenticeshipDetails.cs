using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.CommitmentServiceTests
{
    [TestFixture] 
    public class WhenGettingSeveralIndividualApprenticeshipDetails
    {
        [Test, MoqAutoData]
        public async Task And_We_Find_Matching_Apprenticeships_With_Correct_AccountId_Then_Returns_The_ApprenticeshipDetails(
            long accountId,
            long apprenticeshipId1,
            long apprenticeshipId2,
            [Frozen] Mock<ICommitmentsApiClient<CommitmentsConfiguration>> client,
            [Greedy] CommitmentsService sut)
        {

            var f = new Fixture();
            
            var apprenticeDetails1 = f.Build<ApprenticeshipResponse>()
                .With(x => x.Id, apprenticeshipId1)
                .With(x=>x.ApprenticeshipEmployerTypeOnApproval, ApprenticeshipEmployerType.Levy)
                .With(x => x.EmployerAccountId, accountId)
                .Create();
            
            var apprenticeDetails2 = f.Build<ApprenticeshipResponse>()
                .With(x => x.Id, apprenticeshipId2)
                .With(x => x.ApprenticeshipEmployerTypeOnApproval, ApprenticeshipEmployerType.NonLevy)
                .With(x => x.EmployerAccountId, accountId)
                .Create();

            client.Setup(x => x.Get<ApprenticeshipResponse>(It.Is<IGetApiRequest>(p => 
                    p.GetUrl == $"api/apprenticeships/{apprenticeshipId1}")))
                .ReturnsAsync(apprenticeDetails1);

            client.Setup(x => x.Get<ApprenticeshipResponse>(It.Is<IGetApiRequest>(p =>
                    p.GetUrl == $"api/apprenticeships/{apprenticeshipId2}")))
                .ReturnsAsync(apprenticeDetails2);

            var result = await sut.GetApprenticeshipDetails(accountId, new [] { apprenticeshipId1, apprenticeshipId2} );
            result.Should().BeEquivalentTo(new [] { apprenticeDetails1, apprenticeDetails2 });
        }

        [Test, MoqAutoData]
        public void And_We_Find_Matching_Apprenticeships_With_Different_AccountIds_Then_Throws_UnauthorizedAccessException(
            long accountId,
            long[] apprenticeshipIds,
            ApprenticeshipResponse apprenticeshipDetail,
            [Frozen] Mock<ICommitmentsApiClient<CommitmentsConfiguration>> client,
            [Greedy] CommitmentsService sut)
        {
            client.Setup(x => x.Get<ApprenticeshipResponse>(It.IsAny<IGetApiRequest>()))
                .ReturnsAsync(apprenticeshipDetail);

            Assert.ThrowsAsync<UnauthorizedAccessException>(()=> sut.GetApprenticeshipDetails(accountId, apprenticeshipIds));
        }
    }
}
