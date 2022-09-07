using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Apprenticeships.Api.Models;

public class GetStandardResponse
{
    public string StandardUId { get; set; }
    public string IfateReferenceNumber { get; set; }
    public int LarsCode { get; set; }
    public string Status { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public string Version { get; set; }
    public int VersionMajor { get; set; }
    public int VersionMinor { get; set; }
    public List<string> Options { get; set; }
    public StandardVersionDetail VersionDetail { get; set; }

    public int Duration { get; set; }
    public int MaxFunding { get; set; }
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public DateTime? LastDateForNewStarts { get; set; }
    public List<GetStandardFundingResponse> ApprenticeshipFunding { get; set; }
    public string StandardPageUrl { get; set; }

    public static implicit operator GetStandardResponse(GetStandardsListItem source)
    {
        return new GetStandardResponse
        {
            StandardUId = source.StandardUId,
            IfateReferenceNumber = source.IfateReferenceNumber,
            LarsCode = source.LarsCode,
            Status = source.Status,
            Title = source.Title,
            Level = source.Level,
            Version = source.Version,
            VersionMajor = source.VersionMajor,
            VersionMinor = source.VersionMinor,
            Options = source.Options,
            VersionDetail = source.VersionDetail,
            Duration = source.TypicalDuration,
            MaxFunding = source.MaxFunding,
            EffectiveFrom = source.StandardDates?.EffectiveFrom,
            EffectiveTo = source.StandardDates?.EffectiveTo,
            LastDateForNewStarts = source.StandardDates?.LastDateStarts,
            ApprenticeshipFunding = source.ApprenticeshipFunding.Select(c => (GetStandardFundingResponse)c).ToList(),
            StandardPageUrl = source.StandardPageUrl
        };
    }
}