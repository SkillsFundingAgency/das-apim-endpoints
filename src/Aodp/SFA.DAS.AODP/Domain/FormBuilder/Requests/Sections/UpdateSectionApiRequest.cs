using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Sections;

public class UpdateSectionApiRequest : IPutApiRequest
{
    private readonly Guid _formVersionId;

    public UpdateSectionApiRequest(Guid formVersionId, Section data)
    {
        _formVersionId = formVersionId;
        Data = data;
    }

    public string PutUrl => $"/api/sections/{_formVersionId}";

    public object Data { get; set; }

    public class Section
    {
        public Guid Id { get; set; }
        public Guid FormVersionId { get; set; }
        public Guid Key { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? NextSectionId { get; set; }
    }
}