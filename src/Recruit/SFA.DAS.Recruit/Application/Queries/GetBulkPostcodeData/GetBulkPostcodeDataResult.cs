using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.Domain;


namespace SFA.DAS.Recruit.Application.Queries.GetBulkPostcodeData;

public record GetBulkPostcodeDataItemResult(string Query, PostcodeData Result);

public record GetBulkPostcodeDataResult(List<GetBulkPostcodeDataItemResult> Results);