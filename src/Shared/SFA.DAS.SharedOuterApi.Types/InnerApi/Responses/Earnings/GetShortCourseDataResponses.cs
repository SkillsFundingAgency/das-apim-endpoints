using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;

public class GetShortCourseDataResponse
{
    public List<ShortCourseEarning> Earnings { get; set; }
}

public class ShortCourseEarning
{
    public int CollectionYear { get; set; }
    public byte CollectionPeriod { get; set; }
    public Decimal Amount { get; set; }
    public string Type { get; set; }
}
