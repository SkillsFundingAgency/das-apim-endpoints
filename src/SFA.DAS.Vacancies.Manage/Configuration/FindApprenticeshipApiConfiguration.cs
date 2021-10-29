using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.Manage.Configuration
{
    public class FindApprenticeshipApiConfiguration : IInternalApiConfiguration
    {
    public string Url { get; set; }
    public string Identifier { get; set; }
    }
}
