﻿using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAan.Application.Configuration;

[ExcludeFromCodeCoverage]
public class AanHubApiConfiguration
{
    public string Url { get; set; } = null!;
    public string Identifier { get; set; } = null!;
}