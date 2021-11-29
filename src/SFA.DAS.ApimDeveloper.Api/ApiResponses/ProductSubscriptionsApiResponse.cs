using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProductSubscriptions;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;

namespace SFA.DAS.ApimDeveloper.Api.ApiResponses
{
    public class ProductSubscriptionsApiResponse
    {
        public List<ProductSubscriptionApiResponseItem> Products { get; set; }
        public static implicit operator ProductSubscriptionsApiResponse(GetApiProductSubscriptionsQueryResult source)
        {
            var response =  new ProductSubscriptionsApiResponse
            {
                Products = source.Products.Select(c=>ProductSubscriptionApiResponseItem.Map(c, source.Subscriptions) ).ToList()
            };

            return response;
        }
    }

    public class ProductSubscriptionApiResponseItem
    {
        public string Id { get ; set ; }
        public string Description { get ; set ; }
        public string DisplayName { get ; set ; }
        public string Name { get ; set ; }
        public string Key { get; set; }


        public static ProductSubscriptionApiResponseItem Map(GetAvailableApiProductItem source, IEnumerable<GetApiProductSubscriptionsResponseItem> subscriptions)
        {
            return new ProductSubscriptionApiResponseItem
            {
                Id = source.Id,
                Name = source.Name,
                DisplayName = source.DisplayName,
                Description = source.Description,
                Key = subscriptions.FirstOrDefault(c=>c.Name.Equals(source.Id))?.Key ?? ""
            };
        }
    }
}