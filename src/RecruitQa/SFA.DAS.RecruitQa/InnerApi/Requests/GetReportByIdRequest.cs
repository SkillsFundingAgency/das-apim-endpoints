using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public record GetReportByIdRequest(Guid ReportId) : IGetApiRequest
{
    public string GetUrl => $"api/reports/{ReportId}";
}