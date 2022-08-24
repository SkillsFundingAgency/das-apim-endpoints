using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.TrackProgress.Application.Commands.TrackProgress;
using SFA.DAS.TrackProgress.Application.DTOs;
using SFA.DAS.TrackProgress.Application.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using SFA.DAS.TrackProgress.Application.Services;

namespace SFA.DAS.TrackProgress.Api.Controllers;

[ApiController]
public class Test1Controller : ControllerBase
{
    private readonly CommitmentsV2Service _service;

    public Test1Controller(CommitmentsV2Service service)
    {
        _service = service;
    }

    [HttpGet]
	[Route("/test1")]
    public IActionResult Test()
    {
        return Ok();
    }
}
