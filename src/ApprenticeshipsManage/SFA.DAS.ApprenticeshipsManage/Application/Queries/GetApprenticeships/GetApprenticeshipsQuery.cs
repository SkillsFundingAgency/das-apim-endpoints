using MediatR;
using SFA.DAS.ApprenticeshipsManage.Infrastructure;

namespace SFA.DAS.ApprenticeshipsManage.Application.Queries.GetApprenticeships;
public class GetApprenticeshipsQuery : PagedQuery, IRequest<GetApprenticeshipsQueryResult>
{
    public string Ukprn { get; set; } = "";
    public int AcademicYear { get; set; }
}
