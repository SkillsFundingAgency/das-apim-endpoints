﻿using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Models;

public record WorkExperience
{
    public string Employer { get; set; }

    public string JobTitle { get; set; }

    public string Description { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }
}