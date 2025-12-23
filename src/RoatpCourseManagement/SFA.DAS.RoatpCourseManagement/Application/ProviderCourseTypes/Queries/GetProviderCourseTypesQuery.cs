using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.ProviderCourseTypes.Queries;
public class GetProviderCourseTypesQuery : IRequest<List<ProviderCourseTypeResult>>
{
    public int Ukprn { get; }
    public GetProviderCourseTypesQuery(int ukprn)
    {
        Ukprn = ukprn;
    }
}
