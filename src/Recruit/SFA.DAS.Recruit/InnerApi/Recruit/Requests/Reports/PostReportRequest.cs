using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using static SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports.PostReportRequest;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
public record PostReportRequest(PostReportRequestData Payload) : IPostApiRequest
{
    public string PostUrl => "api/reports";
    public object Data { get; set; } = Payload;

    public record PostReportRequestData
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public Guid UserId { get; init; }
        public string CreatedBy { get; init; } = null!;
        public required DateTime FromDate { get; init; }
        public required DateTime ToDate { get; init; }
        public int? Ukprn { get; init; }
        public required ReportOwnerType OwnerType { get; init; }
    }
}