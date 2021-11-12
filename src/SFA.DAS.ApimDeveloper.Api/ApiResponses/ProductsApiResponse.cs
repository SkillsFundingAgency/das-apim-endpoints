using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;

namespace SFA.DAS.ApimDeveloper.Api.ApiResponses
{
    public class ProductsApiResponse
    {
        public List<ProductsApiResponseItem> Products { get; set; }
        public static implicit operator ProductsApiResponse(GetApiProductsQueryResult source)
        {
            return new ProductsApiResponse
            {
                Products = source.Products.Select(c=>(ProductsApiResponseItem)c).ToList()
            };
        }
    }

    public class ProductsApiResponseItem
    {
        public string Description { get ; set ; }
        public string DisplayName { get ; set ; }
        public string Name { get ; set ; }

        public static implicit operator ProductsApiResponseItem(GetAvailableApiProductItem source)
        {
            return new ProductsApiResponseItem
            {
                Name = source.Name,
                DisplayName = source.DisplayName,
                Description = source.Description,
            };
        }
    }
}