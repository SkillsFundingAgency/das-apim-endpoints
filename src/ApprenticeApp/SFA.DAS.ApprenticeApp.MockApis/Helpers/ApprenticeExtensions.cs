using System;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.MockApis.Helpers
{
    public static class ApprenticeExtensions
    {
        public static string ApprenticeUrlId(this Apprentice apprentice)
            => apprentice == null ||apprentice.ApprenticeId == Guid.Empty ? "*" : apprentice.ApprenticeId.ToString();
    }
}
