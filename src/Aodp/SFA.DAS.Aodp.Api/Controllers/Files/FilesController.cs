using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.Files;
using SFA.DAS.Aodp.Application.Queries.Files;

namespace SFA.DAS.Aodp.Api.Controllers.Files
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : BaseController
    {
        public FilesController(IMediator mediator, ILogger<FilesController> logger) : base(mediator, logger)
        { }


        [HttpPost("create")]
        public async Task<IActionResult> CreateFile([FromBody] CreateFileMetadataCommand command)
        {
            return await SendRequestAsync(command);
        }

        [HttpDelete("{fileId}")]
        public async Task<IActionResult> DeleteFile(Guid fileId)
        {
            return await SendRequestAsync(new DeleteFileMetadataCommand { FileId = fileId });
        }

        [HttpPost]
        public async Task<IActionResult> GetFiles([FromBody] GetFileMetadataQuery fileMetadataQuery)
        {
            return await SendRequestAsync(fileMetadataQuery);
        }

    }
}