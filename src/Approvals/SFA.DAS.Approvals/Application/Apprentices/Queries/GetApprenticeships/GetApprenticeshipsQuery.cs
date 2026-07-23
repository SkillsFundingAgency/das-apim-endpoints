using System;
using MediatR;
using SFA.DAS.Approvals.Enums;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeships;

public class GetApprenticeshipsQuery : GetApprenticeshipsRequest, IRequest<GetApprenticeshipsQueryResult>
{    
}