using SFA.DAS.Aodp.Application.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Specialized;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Jobs
{
    public class GetJobRunsByNameApiRequest : IGetApiRequest
    {
        public string? JobName { get; set; }

        public string BaseUrl = "api/job-runs";

        public string GetUrl
        {
            get
            {
                var queryParams = new NameValueCollection()
                {
                };

                if (!string.IsNullOrWhiteSpace(JobName))
                {
                    queryParams.Add("JobName", JobName);
                }

                var url = BaseUrl.AttachParameters(queryParams);
                return url;
            }
        }
    }
}
