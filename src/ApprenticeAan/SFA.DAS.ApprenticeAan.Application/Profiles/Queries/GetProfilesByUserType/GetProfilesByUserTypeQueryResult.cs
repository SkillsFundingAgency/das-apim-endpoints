﻿using SFA.DAS.ApprenticeAan.Application.Entities;

namespace SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfilesByUserType
{
    public class GetProfilesByUserTypeQueryResult
    {
        public List<Profile> Profiles { get; set; } = new();
    }
}