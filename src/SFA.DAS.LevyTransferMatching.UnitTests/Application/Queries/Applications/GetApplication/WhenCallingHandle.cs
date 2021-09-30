﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplication;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Applications.GetApplication
{
    public class WhenCallingHandle
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test, MoqAutoData]
        public async Task And_Application_Exists_Stitches_Up_Standard_To_Result(
            GetApplicationQuery getApplicationQuery,
            GetApplicationResponse getApplicationResponse,
            GetStandardsListItem getStandardsListItem,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetApplicationQueryHandler getApplicationQueryHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(getApplicationQuery.ApplicationId.ToString()))))
                .ReturnsAsync(getApplicationResponse);

            mockCoursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(y => y.GetUrl.Contains(getApplicationResponse.StandardId))))
                .ReturnsAsync(getStandardsListItem);
            
            var result = await getApplicationQueryHandler.Handle(getApplicationQuery, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(getApplicationResponse.Status, result.Status);
            Assert.AreEqual(getStandardsListItem.Level, result.Level);
        }

        [Test, MoqAutoData]
        public async Task And_Application_Doesnt_Exist_Returns_Null(
            GetApplicationQuery getApplicationQuery,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetApplicationQueryHandler getApplicationQueryHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(getApplicationQuery.ApplicationId.ToString()))))
                .ReturnsAsync((GetApplicationResponse)null);

            var result = await getApplicationQueryHandler.Handle(getApplicationQuery, CancellationToken.None);

            Assert.IsNull(result);
        }
    }
}