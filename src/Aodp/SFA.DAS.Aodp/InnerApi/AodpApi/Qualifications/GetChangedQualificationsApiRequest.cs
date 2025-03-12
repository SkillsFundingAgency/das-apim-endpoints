﻿using SFA.DAS.Aodp.Application.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Specialized;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications
{
    public class GetChangedQualificationsApiRequest : IGetApiRequest
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string? Name { get; set; }
        public string? Organisation { get; set; }
        public string? QAN { get; set; }
        public string BaseUrl = "api/qualifications";

        public string GetUrl
        {
            get
            {
                var queryParams = new NameValueCollection()
                {
                    { "Status", "Changed" },
                };

                if (Skip.HasValue)
                {
                    queryParams.Add("Skip", Skip.ToString());
                }

                if (Take.HasValue)
                {
                    queryParams.Add("Take", Take.ToString());
                }

                if (!string.IsNullOrWhiteSpace(Name))
                {
                    queryParams.Add("Name", Name);
                }

                if (!string.IsNullOrWhiteSpace(Organisation))
                {
                    queryParams.Add("Organisation", Organisation);
                }

                if (!string.IsNullOrWhiteSpace(QAN))
                {
                    queryParams.Add("QAN", QAN);
                }

                var url = BaseUrl.AttachParameters(queryParams);
                return url;
            }
        }
    }

}
