using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public record PostReportRequest(PostReportRequest.PostReportRequestData Payload) : IPostApiRequest
{
    public string PostUrl => "api/reports";
    public object Data { get; set; } = Payload;

    public record PostReportRequestData
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string UserId { get; init; }
        public required string CreatedBy { get; init; }
        public required DateTime FromDate { get; init; }
        public required DateTime ToDate { get; init; }
        public string OwnerType => "Qa";
    }
}
