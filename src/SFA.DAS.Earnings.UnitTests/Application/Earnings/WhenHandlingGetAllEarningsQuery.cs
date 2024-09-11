using AutoFixture;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using FluentAssertions;
using Moq;
using SFA.DAS.Earnings.Application.Earnings;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Earnings.UnitTests.Application.Earnings
{
    public class WhenHandlingGetAllEarningsQuery
    {
        private readonly Fixture _fixture = new();

        [Test, MoqAutoData]
        public async Task Then_Gets_All_Earnings_From_ApiClient(
            GetApprenticeshipsResponse apprenticeshipResponse,
            Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> mockApprenticeshipsApiClient)
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            apprenticeshipResponse.Apprenticeships.ForEach(x => x.Uln = _fixture.Create<long>().ToString());

            mockApprenticeshipsApiClient
                .Setup(x => x.Get<GetApprenticeshipsResponse>(It.Is<GetApprenticeshipsRequest>(r => r.Ukprn == ukprn)))
                .ReturnsAsync(apprenticeshipResponse);

            var handler = new GetAllEarningsQueryHandler(mockApprenticeshipsApiClient.Object);
            var query = new GetAllEarningsQuery { Ukprn = ukprn };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.FM36Learners.Should().BeEquivalentTo(apprenticeshipResponse.Apprenticeships.Select(x => new FM36Learner
            {
                ULN = long.Parse(x.Uln),
                LearnRefNumber = EarningsFM36Constants.LearnRefNumber
            }).ToArray());

            mockApprenticeshipsApiClient.Verify(x => x.Get<GetApprenticeshipsResponse>(It.Is<GetApprenticeshipsRequest>(r => r.Ukprn == ukprn)), Times.Once);
        }
    }

}