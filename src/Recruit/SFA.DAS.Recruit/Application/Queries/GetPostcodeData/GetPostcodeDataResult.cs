namespace SFA.DAS.Recruit.Application.Queries.GetPostcodeData;

public record GetPostcodeDataResult(string Postcode, string Country, double? Latitude, double? Longitude)
{
    public static readonly GetPostcodeDataResult None = new (null, null, null, null);
}