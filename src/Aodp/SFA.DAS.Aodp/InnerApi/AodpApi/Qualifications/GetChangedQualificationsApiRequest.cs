using SFA.DAS.Aodp.Application.Extensions;
using SFA.DAS.Aodp.Models;
using SFA.DAS.Apim.Shared.Interfaces;
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
        public List<Guid> ProcessStatusFilter { get; set; } = new();
        public List<AgeGroup> AgeGroups { get; set; } = new();
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

                if (ProcessStatusFilter?.Count > 0)
                {
                    foreach (var id in ProcessStatusFilter)
                    {
                        queryParams.Add("ProcessStatusFilter", id.ToString());
                    }
                }

                if (AgeGroups?.Count > 0)
                {
                    foreach (var age in AgeGroups)
                    {
                        queryParams.Add("AgeGroups", ((int)age).ToString());
                    }
                }

                var url = BaseUrl.AttachMultiValueParameters(queryParams);
                
                return url;
            }
        }
    }

}
