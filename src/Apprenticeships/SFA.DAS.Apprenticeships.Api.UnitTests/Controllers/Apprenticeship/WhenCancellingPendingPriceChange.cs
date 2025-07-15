﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Controllers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship
{
    public class WhenCancellingPendingPriceChange
    {

	    private Mock<ILearningApiClient<LearningApiConfiguration>> _mockApprenticeshipsApiClient = null!;
	    private ApprenticeshipController _sut = null!;
        private Fixture _fixture = null!;

		[SetUp]
	    public void SetUp()
	    {
            _fixture = new Fixture();

		    _mockApprenticeshipsApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
            _sut = new ApprenticeshipController(Mock.Of<ILogger<ApprenticeshipController>>(), _mockApprenticeshipsApiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(), Mock.Of<IMediator>());
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
