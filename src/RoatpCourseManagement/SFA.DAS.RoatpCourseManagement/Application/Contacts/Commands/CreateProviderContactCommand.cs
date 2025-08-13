using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Contacts.Commands;

public class CreateProviderContactCommand : IRequest<int>
{
    public int Ukprn { get; set; }
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public List<int> ProviderCourseIds { get; set; }
}