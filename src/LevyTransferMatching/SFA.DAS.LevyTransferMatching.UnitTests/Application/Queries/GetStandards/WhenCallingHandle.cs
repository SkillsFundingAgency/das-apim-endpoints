using System.Linq;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Application.Queries.Standards;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetStandards
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task And_No_Id_Specified_Then_All_Standards_Returned(
            GetStandardsListResponse response,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> client,
            GetStandardsQueryHandler getStandardsQueryHandler)
        {
            var getStandardsQuery = new GetStandardsQuery();

            client
                .Setup(x => x.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(response);

            var result = await getStandardsQueryHandler.Handle(getStandardsQuery, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Standards.Count(), Is.EqualTo(response.Standards.Count()));
            client.Verify(x => x.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_Valid_Id_Specified_Then_Standard_Returned(
            GetStandardsListItem response,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> client,
            GetStandardsQueryHandler getStandardsQueryHandler)
        {
            var getStandardsQuery = new GetStandardsQuery() { StandardId = "1"};

            client
                .Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync(response);

            var result = await getStandardsQueryHandler.Handle(getStandardsQuery, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Standards.Count(), Is.EqualTo(1));
        }
    }
}