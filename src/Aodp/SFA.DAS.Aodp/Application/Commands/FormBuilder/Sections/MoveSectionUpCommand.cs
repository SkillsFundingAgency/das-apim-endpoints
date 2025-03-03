using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;

public class MoveSectionUpCommand : IRequest<BaseMediatrResponse<MoveSectionUpCommandResponse>>
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
}
