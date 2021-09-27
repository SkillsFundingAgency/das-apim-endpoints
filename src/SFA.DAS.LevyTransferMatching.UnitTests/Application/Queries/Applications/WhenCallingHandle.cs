using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Applications
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task AndNoApplicationsExistReturnsEmptyList(GetApplicationsQuery query, 
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ILevyTransferMatchingService> levyTransferMatchingService,
            GetApplicationsQueryHandler handler)
        {
            levyTransferMatchingService.Setup(o => o.GetApplications(It.Is<GetApplicationsRequest>(o => o.AccountId == query.AccountId))).ReturnsAsync(
                new GetApplicationsResponse
                {
                    Applications = new List<Models.Application>()
                }
            );

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.AreEqual(0, result.Applications.Count());
            coursesApiClient.Verify(o => o.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task AndApplicationsExistReturnsListOfApplications(GetApplicationsQuery query,
            GetApplicationsResponse response,
            GetStandardsListItem standardsListItem,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ILevyTransferMatchingService> levyTransferMatchingService,
            GetApplicationsQueryHandler handler)
        {
            response.Applications = new List<Models.Application>
            {
                response.Applications.First()
            };

            levyTransferMatchingService.Setup(o => o.GetApplications(It.Is<GetApplicationsRequest>(o => o.AccountId == query.AccountId))).ReturnsAsync(
                new GetApplicationsResponse
                {
                    Applications = response.Applications
                }
            );

            standardsListItem.StandardUId = response.Applications.First().StandardId;

            coursesApiClient.Setup(o => o.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(o => o.Id == response.Applications.First().StandardId)))
                .ReturnsAsync(standardsListItem);

            var result = await handler.Handle(query, CancellationToken.None);

           Assert.IsTrue(result.Applications.Any());
           coursesApiClient.Verify(o => o.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()), Times.Once);
           Assert.AreEqual(response.Applications, response.Applications);
        }
    }
}
