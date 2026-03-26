using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;


public class ShortCourseEarningsResponse
{
    public Guid EarningProfileVersion { get; set; }
    public List<ShortCourseInstalment> Instalments { get; set; } = new();
}

public class ShortCourseInstalment
{
    public short CollectionYear { get; set; }
    public byte CollectionPeriod { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public bool IsPayable { get; set; }
}

public class UpdateShortCourseEarningPutResponse : ShortCourseEarningsResponse
{
}

public class ShortCourseEarningGetResponse : ShortCourseEarningsResponse
{
}