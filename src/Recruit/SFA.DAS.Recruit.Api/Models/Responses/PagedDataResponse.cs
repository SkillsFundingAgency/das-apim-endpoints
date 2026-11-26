using SFA.DAS.Recruit.Data.Models;

namespace SFA.DAS.Recruit.Api.Models.Responses;

public record PageInfo(ushort RequestedPageNumber, ushort RequestedPageSize, uint TotalCount);

public record PagedDataResponse<T>(T Data, PageInfo PageInfo): DataResponse<T>(Data);