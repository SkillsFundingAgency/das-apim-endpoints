using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetRemovedReasons;
public class GetRemovedReasonsQuery : IRequest<GetRemovedReasonsResponse>;