using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
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
                    Applications = new List<GetApplicationsResponse.Application>()
                }
            );

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.AreEqual(0, result.Applications.Count());
            coursesApiClient.Verify(o => o.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task AndApplicationsExistReturnsListOfApplications(GetApplicationsQuery query,
            GetApplicationsResponse response,
            [Frozen] Mock<ILevyTransferMatchingService> levyTransferMatchingService,
            GetApplicationsQueryHandler handler)
        {
            response.Applications = new List<GetApplicationsResponse.Application>()
            {
                response.Applications.First()
            };

            levyTransferMatchingService.Setup(o => o.GetApplications(It.Is<GetApplicationsRequest>(o => o.AccountId == query.AccountId))).ReturnsAsync(
                new GetApplicationsResponse
                {
                    Applications = response.Applications
                }
            );

            var result = await handler.Handle(query, CancellationToken.None);

           Assert.IsTrue(result.Applications.Any());
           Assert.AreEqual(response.Applications, response.Applications);
        }
    }
}
