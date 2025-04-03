using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries;
public class GetUlnSupportApprenticeshipsQuery : IRequest<GetUlnSupportApprenticeshipsQueryResult?>
{
    public string Uln { get; set; }
}