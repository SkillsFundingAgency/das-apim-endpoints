using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetApprenticePreferencesResponse
    {
        public List<ApprenticePreferenceDto> ApprenticePreferences { get; set; } = new List<ApprenticePreferenceDto>();
    }
    public class ApprenticePreferenceDto
    {
        public int PreferenceId { get; set; }
        public string PreferenceMeaning { get; set; }
        public bool Status { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
