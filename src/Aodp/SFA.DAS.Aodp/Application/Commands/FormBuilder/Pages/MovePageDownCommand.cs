using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Pages;

public class MovePageDownCommand : IRequest<BaseMediatrResponse<MovePageDownCommandResponse>>
{
    public Guid PageId { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
}

