using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindEpao.Application.Epaos.Queries.GetDeliveryAreaList
{
    public class GetDeliveryAreaListQueryHandler : IRequestHandler<GetDeliveryAreaListQuery, GetDeliveryAreaListResult>
    {
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;

        public GetDeliveryAreaListQueryHandler(IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient)
        {
            _assessorsApiClient = assessorsApiClient;
        }

        public async Task<GetDeliveryAreaListResult> Handle(GetDeliveryAreaListQuery request, CancellationToken cancellationToken)
        {
            var deliveryAreasList = await _assessorsApiClient.GetAll<GetDeliveryAreaListItem>(new GetDeliveryAreasRequest());
            
            return new GetDeliveryAreaListResult
            {
                DeliveryAreas = deliveryAreasList
            }; 
        }
    }
}