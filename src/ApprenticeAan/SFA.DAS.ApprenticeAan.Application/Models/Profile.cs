﻿namespace SFA.DAS.ApprenticeAan.Application.Models;

public class Profile
{
    public long Id { get; set; }
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public int Ordering { get; set; }
}