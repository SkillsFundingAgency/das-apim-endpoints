using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateOnlineDeliveryOption;
public class UpdateOnlineDeliveryOptionCommand : IRequest<Unit>
{
    public int Ukprn { get; set; }
    public string LarsCode { get; set; }
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
    public bool HasOnlineDeliveryOption { get; set; }
}
