using System.Collections.Generic;
using SFA.DAS.Approvals.Application.DeliveryModels.Queries;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Services;

namespace SFA.DAS.Approvals.UnitTests.Application.DeliveryModels.Queries
{
    public class WhenGettingDeliveryModels
    {
        [Test, MoqAutoData]
        public async Task Then_Delivery_Models_Are_Returned_From_The_Service(
            GetDeliveryModelsQuery query,
            List<string> deliveryModels,
            [Frozen] Mock<IDeliveryModelService> apiClient,
            GetDeliveryModelsQueryHandler handler)
        {
            apiClient
                .Setup(x => x.GetDeliveryModels(query.ProviderId, query.TrainingCode, query.AccountLegalEntityId, null))
                .ReturnsAsync(deliveryModels);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.DeliveryModels.Should().BeEquivalentTo(deliveryModels);
        }
    }
}