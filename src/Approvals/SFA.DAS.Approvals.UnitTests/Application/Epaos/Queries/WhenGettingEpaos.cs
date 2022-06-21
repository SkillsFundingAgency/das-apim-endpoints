using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Epaos.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.Epaos.Queries
{
    public class WhenGettingEpaos
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Epaos_Returned(
            GetEpaosQuery query,
            List<GetEpaosListItem> apiResponse,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> apiClient,
            GetEpaosQueryHandler handler
            )
        {
            apiClient.Setup(x => x.GetAll<GetEpaosListItem>(It.IsAny<GetEpaosRequest>())).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Epaos.Should().BeEquivalentTo(apiResponse);
        }
        
    }
}