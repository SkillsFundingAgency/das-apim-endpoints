using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetCharity;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetCharity
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task Then_GetCharity_from_Reference_Api(
        GetCharityQuery query,
        ApiResponse<GetCharityApiResponse> apiResponse,
        [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockApiClient,
        GetCharityQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.GetWithResponseCode<GetCharityApiResponse>(It.IsAny<GetCharityRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(apiResponse.Body);
        }
    }
}
