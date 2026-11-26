using MediatR;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetRemovedReasons;
public class GetRemovedReasonsQuery : IRequest<GetRemovedReasonsResponse>;