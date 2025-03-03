using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Pages;

public class MovePageUpCommand : IRequest<BaseMediatrResponse<MovePageUpCommandResponse>>
{
    public Guid PageId { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
}
