﻿using System.Diagnostics.CodeAnalysis;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Configuration;

[ExcludeFromCodeCoverage]
public class EarningsApiConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; }
    public string Identifier { get; set; }
}