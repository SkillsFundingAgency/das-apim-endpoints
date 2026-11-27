using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SFA.DAS.RecruitQa.Api.ModelBinders;

public class EnumListModelBinder<T> : IModelBinder where T : struct, Enum
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var values = bindingContext.ValueProvider
            .GetValue(bindingContext.ModelName)
            .Values
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .ToList();

        if (!values.Any())
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        var result = new List<T>();

        foreach (var value in values)
        {
            if (Enum.TryParse<T>(value, ignoreCase: true, out var parsed))
            {
                result.Add(parsed);
            }
            else
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName,
                    $"'{value}' is not a valid value. Valid values are: {string.Join(", ", Enum.GetNames<T>())}");
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
        }

        bindingContext.Result = ModelBindingResult.Success(result);
        return Task.CompletedTask;
    }
}

public class EnumListModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (!context.Metadata.ModelType.IsGenericType) return null;
        if (context.Metadata.ModelType.GetGenericTypeDefinition() != typeof(List<>)) return null;

        var enumType = context.Metadata.ModelType.GetGenericArguments()[0];
        if (!enumType.IsEnum) return null;

        var binderType = typeof(EnumListModelBinder<>).MakeGenericType(enumType);
        return (IModelBinder)Activator.CreateInstance(binderType)!;
    }
}