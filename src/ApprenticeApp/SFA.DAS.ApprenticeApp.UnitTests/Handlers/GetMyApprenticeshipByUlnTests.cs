using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.GetMyApprenticeshipByUln;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetMyApprenticeshipByUlnQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Should_Call_Api_With_Correct_Url_And_Return_MyApprenticeship(
     [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClientMock,
     GetMyApprenticeshipByUlnQueryHandler sut)
        {
            // Arrange
            var query = new GetMyApprenticeshipByUlnQuery
            {
                Uln = 1234567890
            };

            GetMyApprenticeshipByUlnRequest capturedRequest = null;

            var apiResponse = new MyApprenticeship
            {
                Uln = query.Uln
            };

            apiClientMock
                .Setup(c => c.Get<MyApprenticeship>(It.IsAny<GetMyApprenticeshipByUlnRequest>()))
                .Callback<IGetApiRequest>(r => capturedRequest = r as GetMyApprenticeshipByUlnRequest)
                .ReturnsAsync(apiResponse);

            // Act
            var result = await sut.Handle(query, CancellationToken.None);

            // Assert
            result.MyApprenticeship.Uln.Should().Be(query.Uln);

            capturedRequest.Should().NotBeNull();
            capturedRequest.GetUrl.Should().Be($"apprentice/{query.Uln}");

            apiClientMock.Verify(
                c => c.Get<MyApprenticeship>(It.IsAny<GetMyApprenticeshipByUlnRequest>()),
                Times.Once);
        }
    }
}