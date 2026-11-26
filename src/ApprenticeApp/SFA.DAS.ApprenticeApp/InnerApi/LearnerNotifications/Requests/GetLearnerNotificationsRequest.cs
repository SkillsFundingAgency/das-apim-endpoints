using SFA.DAS.Apim.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests
{
    public class GetLearnerNotificationsRequest : IGetApiRequest
    {
        private readonly Guid _accountIdentifier;
        private readonly string _order;
        private readonly DateTime? _dateFrom;
        private readonly List<int> _statuses;
        public GetLearnerNotificationsRequest(
            Guid accountIdentifier, 
            string order = null, 
            DateTime? dateFrom = null,
            List<int> statuses = null)
        {
            _accountIdentifier = accountIdentifier;
            _order = order;
            _dateFrom = dateFrom;
            _statuses = statuses;
        }

        public string GetUrl
        {
            get
            {
                var queryParams = new List<string>();

                if (!string.IsNullOrEmpty(_order))
                    queryParams.Add($"order={_order}");

                if (_dateFrom.HasValue)
                    queryParams.Add($"dateFrom={_dateFrom.Value:O}");

                if (_statuses != null && _statuses.Any())
                {
                    foreach (var status in _statuses)
                    {
                        queryParams.Add($"statuses={status}");
                    }
                }

                var queryString = queryParams.Any()
                    ? $"?{string.Join("&", queryParams)}"
                    : string.Empty;

                return $"learner/{_accountIdentifier}{queryString}";
            }
        }
    }
}