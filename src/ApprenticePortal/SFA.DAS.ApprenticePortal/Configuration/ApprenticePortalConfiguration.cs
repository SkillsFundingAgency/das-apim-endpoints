﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticePortal.Configuration
{
    public class ApprenticePortalConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}