using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Assessors.Application.Queries.GetAllLearners;
using SFA.DAS.Assessors.InnerApi.Requests;
using SFA.DAS.Assessors.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Assessors.UnitTests.Application.Queries
{
    public class WhenHandlingGetAllLearnersQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Learners_From_CommitmentsV2_Api(
            GetAllLearnersQuery query,
            GetAllLearnersResponse apiResponse,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
            GetAllLearnersQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetAllLearnersResponse>(It.IsAny<GetAllLearnersRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Learners.Should().BeEquivalentTo(apiResponse.Learners);
        }
    }
}
