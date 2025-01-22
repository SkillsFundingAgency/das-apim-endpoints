using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;

public class UpdateFormVersionApiRequest : IPutApiRequest
{
    private readonly Guid _formVersionId;

    public UpdateFormVersionApiRequest(Guid formVersionId, FormVersion data)
    {
        _formVersionId = formVersionId;
        Data = data;
    }

    public object Data { get; set; }

    public string PutUrl => $"/api/forms/{_formVersionId}";

    public class FormVersion
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }
        public string Name { get; set; }
        public DateTime Version { get; set; }
        public FormStatus Status { get; set; }
        public bool Archived { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public DateTime DateCreated { get; set; }
    }
}