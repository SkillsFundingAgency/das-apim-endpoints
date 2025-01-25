using SFA.DAS.Aodp.Application;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Sections;

namespace SFA.DAS.AODP.Application.Queries.FormBuilder.Sections;

public class GetAllSectionsQueryResponse : BaseResponse
{
    public List<Section> Data { get; set; }

    public class Section
    {
        public Guid Id { get; set; }
        public Guid FormVersionId { get; set; }
        public Guid Key { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }


        public static implicit operator Section(GetAllSectionsApiResponse.Section entity)
        {
            return (new()
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Order = entity.Order,
                FormVersionId = entity.FormVersionId,
                Key = entity.Key
            });
        }
    }
}
