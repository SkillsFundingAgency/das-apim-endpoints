using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SFA.DAS.Earnings.Application.LearnerRecord;

namespace SFA.DAS.Earnings.Api.Controllers;

public class LearnerDataArrayBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext.HttpContext.Request.ContentType != "application/json")
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid content type");
            return Task.CompletedTask;
        }

        using var reader = new StreamReader(bindingContext.HttpContext.Request.Body);
        var body = reader.ReadToEndAsync().Result;

        try
        {
            var records = JsonSerializer.Deserialize<LearnerRecord[]>(body);
            
            //todo add per record validation.. fluent would be easier
            bindingContext.Result = ModelBindingResult.Success(records);
        }
        catch (Exception ex)
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, ex.Message);
        }

        return Task.CompletedTask;
    }
}