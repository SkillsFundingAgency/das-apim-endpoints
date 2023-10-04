using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Assessors.UnitTests.Application.Queries
{
    public class WhenHandlingSearchApprenticeshipsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Called_And_Count_Returned(
            GetApprenticeshipCountResponse response,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
            SearchApprenticeshipsQueryHandler handler)
        {
            findApprenticeshipApiClient
                .Setup(x => x.Get<GetApprenticeshipCountResponse>(It.IsAny<GetApprenticeshipCountRequest>()))
                .ReturnsAsync(response);

            var actual = await handler.Handle(new SearchApprenticeshipsQuery(), CancellationToken.None);

            actual.TotalApprenticeshipCount.Should().Be(response.TotalVacancies);
        }
    }
}