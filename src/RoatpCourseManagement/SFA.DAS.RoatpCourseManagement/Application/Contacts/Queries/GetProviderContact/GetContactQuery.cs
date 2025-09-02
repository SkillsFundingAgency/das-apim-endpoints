using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RoatpCourseManagement.Application.Contacts.Queries.GetProviderContact;

public class GetContactQuery : IRequest<ApiResponse<GetContactResponse>>
{
    public int Ukprn { get; }

    public GetContactQuery(int ukprn)
    {
        Ukprn = ukprn;
    }
}