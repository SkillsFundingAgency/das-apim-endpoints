using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.EditApprenticeship;
using SFA.DAS.SharedOuterApi.Common;

namespace SFA.DAS.Approvals.Api.Models.Apprentices;

public class GetEditApprenticeshipResponse
{
    public string CourseName { get; set; }
    public bool HasMultipleDeliveryModelOptions { get; set; }
    public bool IsFundedByTransfer { get; set; }
    public short Status { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public LearningType? LearningType { get; set; }

    public static implicit operator GetEditApprenticeshipResponse(GetEditApprenticeshipQueryResult source)
    {
        return new GetEditApprenticeshipResponse
        {
            CourseName = source.CourseName,
            IsFundedByTransfer = source.IsFundedByTransfer,
            HasMultipleDeliveryModelOptions = source.HasMultipleDeliveryModelOptions,
            Status = source.Status,
            LearningType = source.LearningType
        };
    }
}
