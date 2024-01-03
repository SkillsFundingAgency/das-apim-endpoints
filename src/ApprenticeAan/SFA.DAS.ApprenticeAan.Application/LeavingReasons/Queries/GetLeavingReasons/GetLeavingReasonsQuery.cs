using MediatR;
using SFA.DAS.ApprenticeAan.Application.Model;

namespace SFA.DAS.ApprenticeAan.Application.LeavingReasons.Queries.GetLeavingReasons;

public class GetLeavingReasonsQuery : IRequest<List<LeavingCategory>>
{
}