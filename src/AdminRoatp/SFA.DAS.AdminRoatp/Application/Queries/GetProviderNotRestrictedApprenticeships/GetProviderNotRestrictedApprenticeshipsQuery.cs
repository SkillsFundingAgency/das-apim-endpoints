using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProviderNotRestrictedApprenticeships;

public class GetProviderNotRestrictedApprenticeshipsQuery : IRequest<GetProviderNotRestrictedApprenticeshipsResponse>
{
    public int Ukprn { get; set; }
}
