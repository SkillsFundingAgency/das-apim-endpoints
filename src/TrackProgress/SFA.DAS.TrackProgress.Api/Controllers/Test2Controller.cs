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
public class Test2Controller : ControllerBase
{
    private readonly CoursesService _service;

    public Test2Controller(CoursesService service)
    {
        _service = service;
    }

    [HttpGet]
	[Route("/test2")]
    public IActionResult Test()
    {
        return Ok();
    }
}
