using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindEpao.Application.Epaos.Queries.GetDeliveryAreaList;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindEpao.UnitTests.Application.Epaos.Queries.GetDeliveryAreaList
{
    public class WhenHandlingGetDeliveryAreaListQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_DeliveryAreas_From_Assessors_Api(
            GetDeliveryAreaListQuery query,
            List<GetDeliveryAreaListItem> apiResponse,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockApiClient,
            GetDeliveryAreaListQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.GetAll<GetDeliveryAreaListItem>(It.IsAny<GetDeliveryAreasRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.DeliveryAreas.Should().BeEquivalentTo(apiResponse);
        }
    }
}