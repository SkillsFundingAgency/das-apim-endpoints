using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Application.Commands.DeliveryUpdateData
{
    public class DeliveryUpdateCommand :  IRequest<DeliveryUpdateCommandResult>
    {
        public DeliveryUpdate DeliveryUpdate { get; set; }
    }
}