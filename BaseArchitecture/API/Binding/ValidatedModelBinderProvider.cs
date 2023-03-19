using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Binding;

public class ValidatedModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType.IsGenericType
            && context.Metadata.ModelType.GetGenericTypeDefinition() == typeof(Validated<>))
        {
            return (IModelBinder?)Activator.CreateInstance(typeof(ValidatedBodyModelBinder<>)
                .MakeGenericType(context.Metadata.ModelType.GenericTypeArguments));
        }

        return null;
    }
}
