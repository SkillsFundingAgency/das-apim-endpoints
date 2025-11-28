using MediatR;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.Aodp.Application.Commands.Import;

public class ImportPldnsCommand : IRequest<BaseMediatrResponse<ImportPldnsCommandResponse>>
{
    public IFormFile? File { get; set; }
}
