using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Controllers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading.Tasks;
using AutoFixture;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship
{
    public class WhenCancellingPendingPriceChange
    {
	    private Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> _mockApprenticeshipsApiClient = null!;
	    private ApprenticeshipController _sut = null!;
        private Fixture _fixture = null!;

		[SetUp]
	    public void SetUp()
	    {
            _fixture = new Fixture();

		    _mockApprenticeshipsApiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
            _sut = new ApprenticeshipController(_mockApprenticeshipsApiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>());
	    }

        [Test]
        public async Task Then_Deletes_PendingPriceChangePrice_From_ApiClient()
        {
            //  Arrange
            var apprenticeshipKey = _fixture.Create<Guid>();

            //  Act
            var result = await _sut.CancelPendingPriceChange(apprenticeshipKey);

            //  Assert
            result.ShouldBeOfType<OkResult>();
            _mockApprenticeshipsApiClient.Verify(x =>
                x.Delete(It.Is<CancelPendingPriceChangeRequest>(r => r.ApprenticeshipKey == apprenticeshipKey)), Times.Once);
        }
    }
}
