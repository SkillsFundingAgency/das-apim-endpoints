using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProviderRestrictedApprenticeships;

public class GetProviderRestrictedApprenticeshipsQuery : IRequest<GetProviderRestrictedApprenticeshipsResponse>
{
    public int Ukprn { get; set; }
}
