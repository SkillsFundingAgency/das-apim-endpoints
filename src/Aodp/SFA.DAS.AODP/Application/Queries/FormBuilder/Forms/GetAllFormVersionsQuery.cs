using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Forms;

public class GetAllFormVersionsQuery : IRequest<BaseMediatrResponse<GetAllFormVersionsQueryResponse>> { }