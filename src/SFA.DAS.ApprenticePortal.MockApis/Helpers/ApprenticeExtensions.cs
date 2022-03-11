using System;
using SFA.DAS.ApprenticePortal.Models;

namespace SFA.DAS.ApprenticePortal.MockApis.Helpers
{
    public static class ApprenticeExtensions
    {
        public static string ApprenticeUrlId(this Apprentice apprentice)
            => apprentice == null ||apprentice.ApprenticeId == Guid.Empty ? "*" : apprentice.ApprenticeId.ToString();
    }
}
