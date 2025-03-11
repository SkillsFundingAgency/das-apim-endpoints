using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;

public class MoveSectionDownCommand : IRequest<BaseMediatrResponse<MoveSectionDownCommandResponse>>
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
}
