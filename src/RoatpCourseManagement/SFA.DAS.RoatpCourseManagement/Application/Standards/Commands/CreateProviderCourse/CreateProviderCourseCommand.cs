using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.CreateProviderCourse;

public class CreateProviderCourseCommand : IRequest<Unit>
{
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
    public int Ukprn { get; set; }
    public string LarsCode { get; set; }
    public bool? IsApprovedByRegulator { get; set; }
    public string StandardInfoUrl { get; set; }
    public string ContactUsPhoneNumber { get; set; }
    public string ContactUsEmail { get; set; }
    public bool HasNationalDeliveryOption { get; set; }
    public bool HasOnlineDeliveryOption { get; set; }
    public List<ProviderCourseLocationCommandModel> ProviderLocations { get; set; }
    public List<int> SubregionIds { get; set; }
}