using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.ChangeHistory;

public class GetChangeHistoryQuery : IRequest<GetChangeHistoryResponse>
{
}
