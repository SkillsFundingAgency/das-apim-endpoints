using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaos;
using SFA.DAS.EpaoRegister.InnerApi.Requests;
using SFA.DAS.EpaoRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EpaoRegister.UnitTests.Application.Epaos.Queries
{
    public class WhenHandlingGetEpaosQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Epaos_From_Assessor_Api(
            GetEpaosQuery query,
            List<GetEpaosListItem> apiResponse,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetEpaosQueryHandler handler)
        {
            apiResponse[0].Status = EpaoStatus.Live;
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetEpaosListItem>(
                    It.IsAny<GetEpaosRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Epaos.Should().BeEquivalentTo(apiResponse.Where(item => item.Status == EpaoStatus.Live));
        }
    }
}