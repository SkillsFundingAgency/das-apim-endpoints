using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SFA.DAS.EmployerFeedback.InnerApi.Responses
{
    public class GetSettingsResponse : List<GetSettingsItem>
    {
        public DateTime? RefreshALELastRunDate
        {
            get
            {
                var key = SettingsKey.RefreshALELastRunDate.ToString();
                var item = this.FirstOrDefault(x => x.Name == key);
                if (item != null && DateTime.TryParse(item.Value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dt))
                {
                    return dt;
                }
                return null;
            }
        }
    }

    public class GetSettingsItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public enum SettingsKey
    {
        RefreshALELastRunDate
    }
}
