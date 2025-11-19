using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests.DelayedNotifications;

public class DeleteNotificationsByIdsRequest(IEnumerable<long> ids): IDeleteApiRequest
{
    public string DeleteUrl {
        get
        {
            const string baseUrl = "api/notifications";
            return QueryHelpers.AddQueryString(baseUrl,
                new List<KeyValuePair<string, StringValues>> { new("ids", ids.Select(x => $"{x}").ToArray()) });
        }
    }
}