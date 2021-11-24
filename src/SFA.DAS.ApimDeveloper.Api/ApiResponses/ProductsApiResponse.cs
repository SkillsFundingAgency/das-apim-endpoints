using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProducts;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;

namespace SFA.DAS.ApimDeveloper.Api.ApiResponses
{
    public class ProductsApiResponse
    {
        public List<ProductsApiResponseItem> Products { get; set; }
        public static implicit operator ProductsApiResponse(GetApiProductsQueryResult source)
        {
            var response =  new ProductsApiResponse
            {
                Products = source.Products.Select(c=>ProductsApiResponseItem.Map(c, source.Subscriptions) ).ToList()
            };

            return response;
        }
    }

    public class ProductsApiResponseItem
    {
        public string Id { get ; set ; }
        public string Description { get ; set ; }
        public string DisplayName { get ; set ; }
        public string Name { get ; set ; }
        public string Key { get; set; }


        public static ProductsApiResponseItem Map(GetAvailableApiProductItem source, IEnumerable<GetApiProductSubscriptionsResponseItem> subscriptions)
        {
            return new ProductsApiResponseItem
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