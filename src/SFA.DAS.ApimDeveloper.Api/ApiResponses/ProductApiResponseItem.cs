using SFA.DAS.ApimDeveloper.InnerApi.Responses;

namespace SFA.DAS.ApimDeveloper.Api.ApiResponses
{
    public class ProductApiResponseItem
    {
        public string Id { get ; set ; }
        public string Description { get ; set ; }
        public string DisplayName { get ; set ; }
        public string Name { get ; set ; }
        public string Documentation { get; set; }

        public static implicit operator ProductApiResponseItem(GetAvailableApiProductItem source)
        {
            if (source == null)
            {
                return null;
            }
            
            return new ProductApiResponseItem
            {
                Id = source.Id,
                Description = source.Description,
                Documentation = source.Documentation,
                Name = source.Name,
                DisplayName = source.DisplayName
            };
        }
    }
}