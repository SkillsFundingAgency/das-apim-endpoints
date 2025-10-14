using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetAllShortCourseTypes;
public class GetAllShortCourseTypesQuery : IRequest<GetAllCourseTypesResponse>;