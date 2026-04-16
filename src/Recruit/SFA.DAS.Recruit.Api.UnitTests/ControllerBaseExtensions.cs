using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Recruit.Api.UnitTests;

public static class ControllerBaseExtensions
{
    public static void AddControllerContext(this ControllerBase controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }
}