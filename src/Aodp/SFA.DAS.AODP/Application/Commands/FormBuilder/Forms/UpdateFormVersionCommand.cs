using MediatR;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Forms;

public class UpdateFormVersionCommand : IRequest<UpdateFormVersionCommandResponse>
{
    public readonly Guid FormVersionId;
    public readonly FormVersion Data;

    public UpdateFormVersionCommand(Guid formVersionId, FormVersion data)
    {
        FormVersionId = formVersionId;
        Data = data;
    }

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
