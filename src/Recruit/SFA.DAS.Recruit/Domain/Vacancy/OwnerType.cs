﻿using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Domain.Vacancy;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OwnerType
{
    Employer = 0,
    Provider = 1,
    External = 2,
    Unknown = 3
}