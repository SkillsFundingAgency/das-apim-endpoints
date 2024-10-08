using SFA.DAS.EmployerPR.Common;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.EmployerPR.Application.EmployerRelationships.Queries.GetEmployerRelationships;

public sealed class ProviderRequestModel
{
    public required long Ukprn { get; set; }
    public required Guid RequestId { get; set; }
    public Operation[] Operations { get; set; } = [];
    public RequestType RequestType { get; set; }

    public static implicit operator ProviderRequestModel(ProviderRequestsResponseModel source) => new()
    {
        Ukprn = source.Ukprn,
        RequestId = source.RequestId,
        Operations = source.Operations,
        RequestType = source.RequestType
    };
}
